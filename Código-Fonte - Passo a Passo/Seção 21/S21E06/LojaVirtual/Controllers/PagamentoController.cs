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
using LojaVirtual.Models.Contants;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Models.ViewModels.Pagamento;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PagarMe;
using LojaVirtual.Libraries.AutoMapper;
using LojaVirtual.Libraries.Email;
using Microsoft.Extensions.Logging;

namespace LojaVirtual.Controllers
{
    [ClienteAutorizacao]
    [ValidateCookiePagamentoController]
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        private GerenciarPagarMe _gerenciarPagarMe;
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaRepository;
        private GerenciarEmail _gerenciarEmail;
        private ILogger<PagamentoController> _logger;

        public PagamentoController(
            ILogger<PagamentoController> logger,
            GerenciarEmail gerenciarEmail,
            IPedidoRepository pedidoRepository,
            IPedidoSituacaoRepository pedidoSituacaRepository,
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
            _gerenciarEmail = gerenciarEmail;
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaRepository = pedidoSituacaRepository;
            _cookie = cookie;
            _gerenciarPagarMe = gerenciarPagarMe;
            _logger = logger;
        } 

        [HttpGet]
        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
            ValorPrazoFrete frete = ObterFrete();

            ViewBag.Frete = frete;
            ViewBag.Produtos = produtoItemCompleto;
            ViewBag.Parcelamentos = CalcularParcelamento(produtoItemCompleto);

            return View("Index");
        }

        [HttpPost]
        public IActionResult Index([FromForm]IndexViewModel indexViewModel)
        {
            if(ModelState.IsValid)
            {
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete();
                List<ProdutoItem> produtos = CarregarProdutoDB();
                Parcelamento parcela = BuscarParcelamento(produtos, indexViewModel.Parcelamento.Numero);

                try
                {
                    Transaction transaction = _gerenciarPagarMe.GerarPagCartaoCredito(indexViewModel.CartaoCredito, parcela, enderecoEntrega, frete, produtos);
                    Pedido pedido = ProcessarPedido(produtos, transaction);

                    return new RedirectToActionResult("Index", "Pedido", new { id = pedido.Id });
                }
                catch (PagarMeException e)
                {
                    _logger.LogError(e, "PagamentoController > Index");
                    TempData["MSG_E"] = MontarMensagensDeErro(e);

                    return Index();
                }
            }
            else
            {
                return Index();
            }
            
        }
        public IActionResult BoletoBancario()
        {
            EnderecoEntrega enderecoEntrega = ObterEndereco();
            ValorPrazoFrete frete = ObterFrete();
            List<ProdutoItem> produtos = CarregarProdutoDB();
            var valorTotal = ObterValorTotalCompra(produtos);

            try
            {
                Transaction transaction = _gerenciarPagarMe.GerarBoleto(valorTotal, produtos, enderecoEntrega, frete);
                
                Pedido pedido = ProcessarPedido(produtos, transaction);

                return new RedirectToActionResult("Index", "Pedido", new { id = pedido.Id });
            }
            catch (PagarMeException e)
            {
                _logger.LogError(e, "PagamentoController > BoletoBancario");
                TempData["MSG_E"] = MontarMensagensDeErro(e);
                return RedirectToAction(nameof(Index));
            }
        }

        private Pedido ProcessarPedido(List<ProdutoItem> produtos, Transaction transaction)
        {
            TransacaoPagarMe transacaoPagarMe;
            Pedido pedido;

            SalvarPedido(produtos, transaction, out transacaoPagarMe, out pedido);

            SalvarPedidoSituacao(produtos, transacaoPagarMe, pedido);

            DarBaixaNoEstoque(produtos);

            _cookieCarrinhoCompra.RemoverTodos();

            _gerenciarEmail.EnviarDadosDoPedido(_loginCliente.GetCliente(), pedido);

            return pedido;
        }

        private void DarBaixaNoEstoque(List<ProdutoItem> produtos)
        {
            foreach (var produto in produtos)
            {
                Produto produtoDB = _produtoRepository.ObterProduto(produto.Id);
                produtoDB.Estoque -= produto.UnidadesPedidas;

                _produtoRepository.Atualizar(produtoDB);
            }
        }

        private void SalvarPedidoSituacao(List<ProdutoItem> produtos, TransacaoPagarMe transacaoPagarMe, Pedido pedido)
        {
            TransactionProduto tp = new TransactionProduto { Transaction = transacaoPagarMe, Produtos = produtos };
            PedidoSituacao pedidoSituacao = _mapper.Map<Pedido, PedidoSituacao>(pedido);
            pedidoSituacao = _mapper.Map<TransactionProduto, PedidoSituacao>(tp, pedidoSituacao);

            pedidoSituacao.Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO;

            _pedidoSituacaRepository.Cadastrar(pedidoSituacao);
        }

        private void SalvarPedido(List<ProdutoItem> produtos, Transaction transaction, out TransacaoPagarMe transacaoPagarMe, out Pedido pedido)
        {
            transacaoPagarMe = _mapper.Map<TransacaoPagarMe>(transaction);
            pedido = _mapper.Map<TransacaoPagarMe, Pedido>(transacaoPagarMe);
            pedido = _mapper.Map<List<ProdutoItem>, Pedido>(produtos, pedido);

            pedido.Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO;

            _pedidoRepository.Cadastrar(pedido);
        }

        private Parcelamento BuscarParcelamento(List<ProdutoItem> produtos, int numero)
        {
            return _gerenciarPagarMe.CalcularPagamentoParcelado(ObterValorTotalCompra(produtos)).Where(a => a.Numero == numero).First();
        }
        private EnderecoEntrega ObterEndereco()
        {
            EnderecoEntrega enderecoEntrega = null;
            var enderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

            if (enderecoEntregaId == 0)
            {
                Cliente cliente = _loginCliente.GetCliente();
                enderecoEntrega = _mapper.Map<EnderecoEntrega>(cliente);
            }
            else
            {
                var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
                enderecoEntrega = endereco;
            }

            return enderecoEntrega;
        }
        private ValorPrazoFrete ObterFrete()
        {
            var enderecoEntrega = ObterEndereco();
            int cep = int.Parse(Mascara.Remover(enderecoEntrega.CEP));
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());
            
            Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();

            if (frete != null)
            {
                return frete.ListaValores.Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            }
            return null;
        }
        private decimal ObterValorTotalCompra(List<ProdutoItem> produtos)
        {
            ValorPrazoFrete frete = ObterFrete();
            decimal total = Convert.ToDecimal( frete.Valor );

            foreach (var produto in produtos)
            {
                total += produto.Valor * produto.UnidadesPedidas;
            }

            return total;
        }
        private List<SelectListItem> CalcularParcelamento(List<ProdutoItem> produtos)
        {
            var total = ObterValorTotalCompra(produtos);
            var parcelamento = _gerenciarPagarMe.CalcularPagamentoParcelado(total);

            
            return parcelamento.Select(a => new SelectListItem(
                String.Format(
                    "{0}x {1} {2} - TOTAL: {3}",
                    a.Numero, a.ValorPorParcela.ToString("C"), (a.Juros) ? "c/ juros" : "s/ juros", a.Valor.ToString("C")
                ),
                a.Numero.ToString()
            )).ToList();
        }
        private string MontarMensagensDeErro(PagarMeException e)
        {
            StringBuilder sb = new StringBuilder();

            if (e.Error.Errors.Count() > 0)
            {
                sb.Append("Erro no pagamento: ");
                foreach (var erro in e.Error.Errors)
                {
                    sb.Append("- " + erro.Message + "<br />");
                }
            }
            return sb.ToString();
        }
    }
}