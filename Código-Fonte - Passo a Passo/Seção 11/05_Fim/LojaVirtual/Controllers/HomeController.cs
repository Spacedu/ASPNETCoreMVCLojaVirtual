using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LojaVirtual.Database;
using LojaVirtual.Repositories.Contracts;

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _repositoryCliente;
        private INewsletterRepository _repositoryNewsletter;
        public HomeController(IClienteRepository repositoryCliente, INewsletterRepository repositoryNewsletter)
        {
            _repositoryCliente = repositoryCliente;
            _repositoryNewsletter = repositoryNewsletter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if (ModelState.IsValid)
            {
                _repositoryNewsletter.Cadastrar(newsletter);
                
                TempData["MSG_S"] = "E-mail cadastrado! Agora você vai receber promoções especiais no seu e-mail! Fique atento as novidades!";
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Contato()
        {
            return View();
        }
        public IActionResult ContatoAcao()
        {
            try
            {
                Contato contato = new Contato();
                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];
                
                var listaMensagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                bool isValid = Validator.TryValidateObject(contato, contexto, listaMensagens, true);

                if (isValid)
                {
                    ContatoEmail.EnviarContatoPorEmail(contato);

                    ViewData["MSG_S"] = "Mensagem de contato enviado com sucesso!";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var texto in listaMensagens)
                    {
                        sb.Append(texto.ErrorMessage + "<br />");
                    }

                    ViewData["MSG_E"] = sb.ToString();
                    ViewData["CONTATO"] = contato;
                }

                
            }
            catch (Exception e)
            {
                ViewData["MSG_E"] = "Opps! Tivemos um erro, tente novamente mais tarde!";

                //TODO - Implementar Log
            }
            

            return View("Contato");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Cliente cliente)
        {
            if(cliente.Email == "elias.ribeiro.s@gmail.com" && cliente.Senha == "1234")
            {
                return new ContentResult() { Content="Logado!"};
            }
            else
            {
                return new ContentResult() { Content = "Não logado!" };
            }
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastroCliente([FromForm]Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _repositoryCliente.Cadastrar(cliente);
                
                TempData["MSG_S"] = "Cadastro realizado com sucesso!";

                //TODO - Implementar redirecionamentos diferentes (Painel, Carrinho de Compras etc).
                return RedirectToAction(nameof(CadastroCliente));
            }
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}