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
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        public PagamentoController(Cookie cookie, CookieCarrinhoCompra carrinhoCompra, IEnderecoEntregaRepository enderecoEntregaRepository, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote, CookieFrete cookieValorPrazoFrete) : base(carrinhoCompra, enderecoEntregaRepository, produtoRepository, mapper, wscorreios, calcularPacote, cookieValorPrazoFrete)
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

                var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
                var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());
                var cep = endereco.CEP;

                _cookieFrete.Consultar()
                
                var frete = .Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();

                if (frete != null)
                {
                    ViewBag.Frete = frete;
                    List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

                    return View(produtoItemCompleto);
                }
            }

            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("Index", "CarrinhoCompra");
        }
    }
}