using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LojaVirtual.Controllers.Base;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Cookie;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Texto;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Models.ViewModels.Pagamento;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagarMe;

namespace LojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        private GerenciarPagarMe _gerenciarPagarMe;

        public PagamentoController(
            GerenciarPagarMe gerenciarPagarMe,
            LoginCliente loginCliente,
            Cookie cookie, 
            CookieCarrinhoCompra carrinhoCompra, 
            IEnderecoEntregaRepository enderecoEntregaRepository, 
            IProdutoRepository produtoRepository, 
            IMapper mapper, 
            WSCorreiosCalcularFrete wscorreios, 
            CalcularPacote calcularPacote, 
            CookieFrete cookieValorPrazoFrete) 
            : base(
                  loginCliente,
                  carrinhoCompra, 
                  enderecoEntregaRepository, 
                  produtoRepository, 
                  mapper, 
                  wscorreios, 
                  calcularPacote, 
                  cookieValorPrazoFrete)
        {
            _cookie = cookie;
            _gerenciarPagarMe = gerenciarPagarMe;
        } 

        [ClienteAutorizacao]
        [HttpGet]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                var enderecoEntrega = ObterEndereco();
                var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());
                int cep = int.Parse(Mascara.Remover( enderecoEntrega.CEP ));
                List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                var frete = ObterFrete(cep.ToString());

                var total = ObterValorTotalCompra(produtoItemCompleto, frete);
                var parcelamento = _gerenciarPagarMe.CalcularPagamentoParcelado(total);

                ViewBag.Frete = frete;
                ViewBag.Produtos = produtoItemCompleto;
                ViewBag.Parcelamentos = parcelamento.Select(a => new SelectListItem(
                    String.Format(
                        "{0}x {1} {2} - TOTAL: {3}", 
                        a.Numero, a.ValorPorParcela.ToString("C"), (a.Juros) ? "c/ juros" : "s/ juros", a.Valor.ToString("C")
                    ),
                    a.Numero.ToString()
                )).ToList();

                return View("Index");
            }
            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("EnderecoEntrega", "CarrinhoCompra");
        }

        [HttpPost]
        [ClienteAutorizacao]
        public IActionResult Index([FromForm]IndexViewModel indexViewModel)
        {
            if(ModelState.IsValid)
            {
                //TODO - Integrar com Pagar.me, Salvar o pedido (Class), Redirecionar para a tela de pedido concluido;
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete(enderecoEntrega.CEP.ToString());
                List<ProdutoItem> produtos = CarregarProdutoDB();
                var parcela = _gerenciarPagarMe.CalcularPagamentoParcelado(ObterValorTotalCompra(produtos, frete)).Where(a=>a.Numero == indexViewModel.Parcelamento.Numero).First();

                try
                {
                    dynamic pagarmeResposta = _gerenciarPagarMe.GerarPagCartaoCredito(indexViewModel.CartaoCredito, enderecoEntrega, frete, produtos);

                    return new ContentResult() { Content = "Sucesso! " + pagarmeResposta.TransactionId };
                }
                catch(PagarMeException e)
                {
                    StringBuilder sb = new StringBuilder();

                    if( e.Error.Errors.Count() > 0)
                    {
                        sb.Append("Erro no pagamento: ");
                        foreach(var erro in e.Error.Errors)
                        {
                            sb.Append("- " + erro.Message + "<br />");
                        }
                    }
                    TempData["MSG_E"] = sb.ToString();

                    return Index();
                }
                

                
            }
            else
            {
                return Index();
            }
            
        }


        private EnderecoEntrega ObterEndereco()
        {
            EnderecoEntrega enderecoEntrega = null;
            var enderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

            if (enderecoEntregaId == 0)
            {
                Cliente cliente = _loginCliente.GetCliente();
                enderecoEntrega = new EnderecoEntrega();
                enderecoEntrega.Nome = "Endereço do cliente";
                enderecoEntrega.Id = 0;
                enderecoEntrega.CEP = cliente.CEP;
                enderecoEntrega.Estado = cliente.Estado;
                enderecoEntrega.Cidade = cliente.Cidade;
                enderecoEntrega.Bairro = cliente.Bairro;
                enderecoEntrega.Endereco = cliente.Endereco;
                enderecoEntrega.Complemento = cliente.Complemento;
                enderecoEntrega.Numero = cliente.Numero;                
            }
            else
            {
                var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
            }

            return enderecoEntrega;
        }
        private ValorPrazoFrete ObterFrete(string cepDestino)
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());
            int cep = int.Parse(Mascara.Remover(cepDestino));

            Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();

            if (frete != null)
            {
                return frete.ListaValores.Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            }
            return null;
        }

        private decimal ObterValorTotalCompra(List<ProdutoItem> produtos, ValorPrazoFrete frete)
        {
            decimal total = Convert.ToDecimal( frete.Valor );

            foreach (var produto in produtos)
            {
                total += produto.Valor;
            }

            return total;
        }
    }
}