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

        public IActionResult NFE([FromForm]VisualizarViewModel viewModel, int id)
        {
            ModelState.Remove("Pedido");
            ModelState.Remove("CodigoRastreamento");
            ModelState.Remove("CartaoCredito");
            ModelState.Remove("BoletoBancario");
            ModelState.Remove("Devolucao");
            ModelState.Remove("DevolucaoMotivoRejeicao");

            if (ModelState.IsValid)
            {
                string url = viewModel.NFE.NFE_URL;

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
            }
            else
            {
                ViewBag.MODAL_NFE = true;
            }
            
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }
        public IActionResult RegistrarRastreamento([FromForm]VisualizarViewModel viewModel, int id)
        {
            ModelState.Remove("Pedido");
            ModelState.Remove("NFE");
            ModelState.Remove("CartaoCredito");
            ModelState.Remove("BoletoBancario");
            ModelState.Remove("Devolucao");
            ModelState.Remove("DevolucaoMotivoRejeicao");

            if (ModelState.IsValid)
            {
                string codRastreamento = viewModel.CodigoRastreamento.Codigo;

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
            }
            else
            {
                ViewBag.MODAL_RASTREAMENTO = true;
            }

            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }


        public IActionResult RegistrarCancelamentoPedidoCartaoCredito([FromForm]VisualizarViewModel viewModel, int id)
        {
            ModelState.Remove("Pedido");
            ModelState.Remove("NFE");
            ModelState.Remove("CodigoRastreamento");
            ModelState.Remove("BoletoBancario");
            ModelState.Remove("Devolucao");
            ModelState.Remove("DevolucaoMotivoRejeicao");

            if (ModelState.IsValid)
            {
                viewModel.CartaoCredito.FormaPagamento = MetodoPagamentoConstant.CartaoCredito;

                Pedido pedido = _pedidoRepository.ObterPedido(id);
                _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);

                pedido.Situacao = PedidoSituacaoConstant.ESTORNO;

                var pedidoSituacao = new PedidoSituacao();
                pedidoSituacao.Data = DateTime.Now;
                pedidoSituacao.Dados = JsonConvert.SerializeObject(viewModel.CartaoCredito);
                pedidoSituacao.PedidoId = id;
                pedidoSituacao.Situacao = PedidoSituacaoConstant.ESTORNO;

                _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
                _pedidoRepository.Atualizar(pedido);

                DevolverProdutosEstoque(pedido);
            }
            else
            {
                ViewBag.MODAL_CARTAOCREDITO = true;
            }

            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }


        public IActionResult RegistrarCancelamentoPedidoBoletoBancario([FromForm]VisualizarViewModel viewModel, int id)
        {
            ModelState.Remove("Pedido");
            ModelState.Remove("NFE");
            ModelState.Remove("CartaoCredito");
            ModelState.Remove("CodigoRastreamento");
            ModelState.Remove("Devolucao");
            ModelState.Remove("DevolucaoMotivoRejeicao");

            if (ModelState.IsValid)
            {
                viewModel.BoletoBancario.FormaPagamento = MetodoPagamentoConstant.Boleto;

                Pedido pedido = _pedidoRepository.ObterPedido(id);
                _gerenciarPagarMe.EstornoBoletoBancario(pedido.TransactionId, viewModel.BoletoBancario);

                pedido.Situacao = PedidoSituacaoConstant.ESTORNO;

                var pedidoSituacao = new PedidoSituacao();
                pedidoSituacao.Data = DateTime.Now;
                pedidoSituacao.Dados = JsonConvert.SerializeObject(viewModel.BoletoBancario);
                pedidoSituacao.PedidoId = id;
                pedidoSituacao.Situacao = PedidoSituacaoConstant.ESTORNO;

                _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
                _pedidoRepository.Atualizar(pedido);

                DevolverProdutosEstoque(pedido);
            }
            else
            {
                ViewBag.MODAL_BOLETOBANCARIO = true;
            }

            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }



        public IActionResult RegistrarDevolucaoPedido([FromForm]VisualizarViewModel viewModel, int id)
        {
            ModelState.Remove("Pedido");
            ModelState.Remove("NFE");
            ModelState.Remove("CartaoCredito");
            ModelState.Remove("CodigoRastreamento");
            ModelState.Remove("BoletoBancario");
            ModelState.Remove("DevolucaoMotivoRejeicao");

            if (ModelState.IsValid)
            {
                Pedido pedido = _pedidoRepository.ObterPedido(id);
                pedido.Situacao = PedidoSituacaoConstant.DEVOLVER;

                var pedidoSituacao = new PedidoSituacao();
                pedidoSituacao.Data = DateTime.Now;
                pedidoSituacao.Dados = JsonConvert.SerializeObject(viewModel.Devolucao);
                pedidoSituacao.PedidoId = id;
                pedidoSituacao.Situacao = PedidoSituacaoConstant.DEVOLVER;

                _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
                _pedidoRepository.Atualizar(pedido);
            }
            else
            {
                ViewBag.MODAL_DEVOLVER = true;
            }

            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarDevolucaoPedidoRejeicao([FromForm]VisualizarViewModel viewModel, int id)
        {
            ModelState.Remove("Pedido");
            ModelState.Remove("NFE");
            ModelState.Remove("CartaoCredito");
            ModelState.Remove("CodigoRastreamento");
            ModelState.Remove("BoletoBancario");

            if (ModelState.IsValid)
            {
                Pedido pedido = _pedidoRepository.ObterPedido(id);
                pedido.Situacao = PedidoSituacaoConstant.DEVOLUCAO_REJEITADA;

                var pedidoSituacao = new PedidoSituacao();
                pedidoSituacao.Data = DateTime.Now;
                pedidoSituacao.Dados = viewModel.DevolucaoMotivoRejeicao;
                pedidoSituacao.PedidoId = id;
                pedidoSituacao.Situacao = PedidoSituacaoConstant.DEVOLUCAO_REJEITADA;

                _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
                _pedidoRepository.Atualizar(pedido);
            }
            else
            {
                ViewBag.MODAL_DEVOLVER_REJEITAR = true;
            }

            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);

        }

        public IActionResult RegistrarDevolucaoPedidoAprovadoCartaoCredito(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);
            if (pedido.Situacao == PedidoSituacaoConstant.DEVOLVER_ENTREGUE)
            {
                var pedidoSituacao = new PedidoSituacao();
                pedidoSituacao.Data = DateTime.Now;
                pedidoSituacao.PedidoId = id;
                pedidoSituacao.Situacao = PedidoSituacaoConstant.DEVOLUCAO_ACEITA;
                _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);

                pedidoSituacao = new PedidoSituacao();
                pedidoSituacao.Data = DateTime.Now;
                pedidoSituacao.PedidoId = id;
                pedidoSituacao.Situacao = PedidoSituacaoConstant.ESTORNO;
                _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                DevolverProdutosEstoque(pedido);

                pedido.Situacao = PedidoSituacaoConstant.ESTORNO;
                _pedidoRepository.Atualizar(pedido);
            }
            VisualizarViewModel viewModel = new VisualizarViewModel();
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
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