using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models.Contants;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Controllers.Base;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Filtro;
using Newtonsoft.Json;
using LojaVirtual.Libraries.Seguranca;
using Microsoft.Extensions.Logging;

namespace LojaVirtual.Controllers
{
    public class CarrinhoCompraController : BaseController
    {
        private ILogger _logger;
        public CarrinhoCompraController(
            ILogger<CarrinhoCompraController> logger,
            LoginCliente loginCliente, 
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
                  cookieValorPrazoFrete
            )
        { _logger = logger; }
        
        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

            return View(produtoItemCompleto);
        }

        public IActionResult VerificarEstoque()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

            foreach (var produto in produtoItemCompleto)
            {
                if(produto.Estoque <= 0)
                {
                    ViewBag.MSG_E = Mensagem.MSG_E008;
                    return View("Index", produtoItemCompleto);
                }
                if (produto.Estoque < produto.UnidadesPedidas)
                {
                    ViewBag.MSG_E = Mensagem.MSG_E008;
                    return View("Index", produtoItemCompleto);
                }
            }
            return RedirectToAction(nameof(EnderecoEntrega));
        }
        //Item ID = ID Produto
        public IActionResult AdicionarItem(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if(produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                var item = new ProdutoItem() { Id = id, UnidadesPedidas = 1 };
                _cookieCarrinhoCompra.Cadastrar(item);

                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
            Produto produto = _produtoRepository.ObterProduto(id);
            if(quantidade < 1)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E007 });
            }else if (quantidade > produto.Estoque)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E008 });
            }
            else
            {
                var item = new ProdutoItem() { Id = id, UnidadesPedidas = quantidade };
                _cookieCarrinhoCompra.Atualizar(item);
                return Ok(new { mensagem = Mensagem.MSG_S001 });
            }
        }
        public IActionResult RemoverItem(int id)
        {
            _cookieCarrinhoCompra.Remover(new ProdutoItem() { Id = id });
            return RedirectToAction(nameof(Index));
        }

        [ClienteAutorizacao]
        public IActionResult EnderecoEntrega()
        {
            Cliente cliente = _loginCliente.GetCliente();
            IList<EnderecoEntrega> enderecos = _enderecoEntregaRepository.ObterTodosEnderecoEntregaCliente(cliente.Id);

            ViewBag.Produtos = CarregarProdutoDB();
            ViewBag.Cliente = cliente;
            ViewBag.Enderecos = enderecos;

            return View();
        }


        public async Task<IActionResult> CalcularFrete(int cepDestino)
        {
            try {
                //Verifica se existe no Frete o calculo para o mesmo CEP e produtos.
                Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cepDestino && a.CodCarrinho == GerarHash(_cookieCarrinhoCompra.Consultar())).FirstOrDefault();
                if(frete != null)
                {
                    return Ok(frete);
                } else { 

                    List<ProdutoItem> produtos = CarregarProdutoDB();
                    List<Pacote> pacotes = _calcularPacote.CalcularPacotesDeProdutos(produtos);

                    ValorPrazoFrete valorPAC = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.PAC, pacotes);
                    ValorPrazoFrete valorSEDEX = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX, pacotes);
                    ValorPrazoFrete valorSEDEX10 = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX10, pacotes);

                    List<ValorPrazoFrete> lista = new List<ValorPrazoFrete>();
                    if(valorPAC != null) lista.Add(valorPAC);
                    if(valorSEDEX != null) lista.Add(valorSEDEX);
                    if(valorSEDEX10 != null) lista.Add(valorSEDEX10);
                
                    frete = new Frete()
                    {
                        CEP = cepDestino,
                        CodCarrinho = GerarHash( _cookieCarrinhoCompra.Consultar() ),
                        ListaValores = lista
                    };
                
                    _cookieFrete.Cadastrar(frete);

                    return Ok(frete);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CarrinhoCompraControler > CalcularFrete");
                return BadRequest(e);
            }
        }
    }
}