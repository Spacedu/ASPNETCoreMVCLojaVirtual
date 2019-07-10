using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using LojaVirtual.Libraries.Json.Resolver;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Models.ViewModels.Pedido;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private GerenciarPagarMe _gerenciarPagarMe;
        private IProdutoRepository _produtoRepository;

        //TODO - Remover ProdutoRepository (Quando refatorar DevolverProdutoEstoque)
        public PedidoController(IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository, GerenciarPagarMe gerenciarPagarMe, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _gerenciarPagarMe = gerenciarPagarMe;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index(int? pagina, string codigoPedido, string cpf)
        {
            var pedidos = _pedidoRepository.ObterTodosPedido(pagina, codigoPedido, cpf);

            return View(pedidos);
        }

        public IActionResult Visualizar(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            var viewModel = new VisualizarViewModel() { Pedido = pedido };

            return View(viewModel);
        }

        public IActionResult NFE(int id)
        {
            string url = HttpContext.Request.Form["nfe_url"];

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            pedido.NFE = url;
            pedido.Situacao = PedidoSituacaoConstant.NF_EMITIDA;

            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            pedidoSituacao.Dados = url;
            pedidoSituacao.PedidoId = id;
            pedidoSituacao.Situacao = PedidoSituacaoConstant.NF_EMITIDA;

            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

            _pedidoRepository.Atualizar(pedido);

            return RedirectToAction(nameof(Visualizar), new { id = id });
        }
        public IActionResult RegistrarRastreamento(int id)
        {
            string codRastreamento = HttpContext.Request.Form["cod_rastreamento"];

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            pedido.FreteCodRastreamento = codRastreamento;
            pedido.Situacao = PedidoSituacaoConstant.EM_TRANSPORTE;

            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            pedidoSituacao.Dados = codRastreamento;
            pedidoSituacao.PedidoId = id;
            pedidoSituacao.Situacao = PedidoSituacaoConstant.EM_TRANSPORTE;

            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

            _pedidoRepository.Atualizar(pedido);

            return RedirectToAction(nameof(Visualizar), new { id = id });
        }


        public IActionResult RegistrarCancelamentoPedidoCartaoCredito(int id)
        {
            string motivo = HttpContext.Request.Form["motivo"];

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);

            pedido.Situacao = PedidoSituacaoConstant.ESTORNO;

            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            
            pedidoSituacao.Dados = JsonConvert.SerializeObject(new DadosCancelamento() { Motivo = motivo });
            pedidoSituacao.PedidoId = id;
            pedidoSituacao.Situacao = PedidoSituacaoConstant.ESTORNO;

            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
            _pedidoRepository.Atualizar(pedido);

            DevolverProdutosEstoque(pedido);

            return RedirectToAction(nameof(Visualizar), new { id = id });
        }

        //TODO - Refatorar - Centralizar código DevolverProdutosEstoque(PedidoPagamentoSituacao) com PedidoController
        private void DevolverProdutosEstoque(Pedido pedido)
        {
            List<ProdutoItem> produtos = JsonConvert.DeserializeObject<List<ProdutoItem>>(pedido.DadosProdutos, new JsonSerializerSettings() { ContractResolver = new ProdutoItemResolver<List<ProdutoItem>>() });

            //TODO - Renomear - Quantidade Produto (QuantidadeEstoque)
            //TODO - Renomear - QuantidadeProdutoCarrinho (QuantidadeComprada)
            foreach (var produto in produtos)
            {
                Produto produtoDB = _produtoRepository.ObterProduto(produto.Id);
                produtoDB.Quantidade += produto.QuantidadeProdutoCarrinho;

                _produtoRepository.Atualizar(produtoDB);
            }

        }
    }
}