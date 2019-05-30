using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LojaVirtual.Controllers.Base;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Cookie;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Texto;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        public PagamentoController(
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
        } 

        [ClienteAutorizacao]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            if(tipoFreteSelecionadoPeloUsuario != null)
            {
                var enderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

                int cep = 0;
                if(enderecoEntregaId == 0)
                {
                    cep = int.Parse(Mascara.Remover(_loginCliente.GetCliente().CEP));
                }
                else { 
                    var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
                    cep = int.Parse(Mascara.Remover(endereco.CEP));
                }
                var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());

                Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();

                if (frete != null)
                {
                    ViewBag.Frete = frete.ListaValores.Where(a=>a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
                    List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                    ViewBag.Produtos = produtoItemCompleto;
                    return View();
                }
            }

            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("Index", "CarrinhoCompra");
        }
    }
}