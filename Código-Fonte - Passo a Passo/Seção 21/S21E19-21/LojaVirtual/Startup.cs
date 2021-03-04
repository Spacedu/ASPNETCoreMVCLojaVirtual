using LojaVirtual.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Repositories;
using LojaVirtual.Repositories.Contracts;
using LojaVirtual.Libraries.Sessao;
using LojaVirtual.Libraries.Login;
using System.Net.Mail;
using System.Net;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Libraries.Middleware;
using LojaVirtual.Libraries.CarrinhoCompra;
using AutoMapper;
using LojaVirtual.Libraries.AutoMapper;
using LojaVirtual.Libraries.Gerenciador.Frete;
using WSCorreios;
using LojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using Coravel;
using LojaVirtual.Libraries.Gerenciador.Scheduler.Invocable;
using Microsoft.Extensions.Hosting;

namespace LojaVirtual
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * API - Logging
             */
            services.AddLogging();
            /*
             * AutoMapper
             */
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
            services.AddAutoMapper(config=>config.AddProfile<MappingProfile>());
#pragma warning restore CS0618 // O tipo ou membro é obsoleto

            /*
             * Padrão Repository
             */
            services.AddHttpContextAccessor();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IImagemRepository, ImagemRepository>();
            services.AddScoped<IEnderecoEntregaRepository, EnderecoEntregaRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoSituacaoRepository, PedidoSituacaoRepository>();

            /*
             * SMTP
             */
            services.AddScoped<SmtpClient>(options =>
            {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = Configuration.GetValue<string>("Email:ServerSMTP"),
                    Port = Configuration.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Configuration.GetValue<string>("Email:Username"), Configuration.GetValue<string>("Email:Password")),
                    EnableSsl = true
                };

                return smtp;
            });
            services.AddScoped<CalcPrecoPrazoWSSoap>(options => {
                var servico = new CalcPrecoPrazoWSSoapClient(CalcPrecoPrazoWSSoapClient.EndpointConfiguration.CalcPrecoPrazoWSSoap);
                return servico;
            });
            services.AddScoped<GerenciarEmail>();
            services.AddScoped<LojaVirtual.Libraries.Cookie.Cookie>();
            services.AddScoped<CookieCarrinhoCompra>();
            services.AddScoped<CookieFrete>();
            services.AddScoped<CalcularPacote>();
            services.AddScoped<WSCorreiosCalcularFrete>();

            services.Configure<CookiePolicyOptions>(options =>
            {

                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            /*
             * Session - Configuração
             */
            services.AddMemoryCache(); //Guardar os dados na memória
            

            services.AddScoped<Sessao>();
            services.AddScoped<LojaVirtual.Libraries.Cookie.Cookie>();
            services.AddScoped<LoginCliente>();
            services.AddScoped<LoginColaborador>();
            services.AddScoped<GerenciarPagarMe>();

            services.AddMvc(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "O campo deve ser preenchido!");
            })
            .AddSessionStateTempDataProvider();

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<LojaVirtualContext>(options => options.UseSqlServer(connection));
            

            services.AddTransient<PedidoPagamentoSituacaoJob>();
            services.AddTransient<PedidoEntregueJob>();
            services.AddTransient<PedidoFinalizadoJob>();
            services.AddTransient<PedidoDevolverEntregueJob>();
            services.AddScheduler();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error/Error500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error/Error500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseSession();
            app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

            /*
             * https://www.site.com.br -> Qual controlador? (Gestão) -> Rotas
             * https://www.site.com.br/Produto/Visualizar/MouseRazorZK
             * https://www.site.com.br/Produto/Visualizar/10
             * https://www.site.com.br/Produto/Visualizar -> Listagem de todos os produtos
             * 
             * https://www.site.com.br -> https://www.site.com.br/Home/Index
             * https://www.site.com.br/Produto -> https://www.site.com.br/Produto/Index
             */
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                  );
                endpoint.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}");
            });

            /*
             Scheduler - Coravel
             */
            app.ApplicationServices.UseScheduler(scheduler => {
                scheduler.Schedule<PedidoPagamentoSituacaoJob>().EveryTenSeconds();
                scheduler.Schedule<PedidoEntregueJob>().EveryTenSeconds();
                scheduler.Schedule<PedidoFinalizadoJob>().EveryTenSeconds();
                scheduler.Schedule<PedidoDevolverEntregueJob>().EveryTenSeconds();
            });


        }
    }
}
