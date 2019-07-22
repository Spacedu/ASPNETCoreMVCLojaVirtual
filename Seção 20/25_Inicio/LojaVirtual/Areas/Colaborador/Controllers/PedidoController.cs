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
            ValidarFormulario(nameof(viewModel.NFE));

            if (ModelState.IsValid)
            {
                string url = viewModel.NFE.NFE_URL;

                Pedido pedido = _pedidoRepository.ObterPedido(id);
                pedido.NFE = url;
                pedido.Situacao = PedidoSituacaoConstant.NF_EMITIDA;

                SalvarPedidoSituacao(id, url, PedidoSituacaoConstant.NF_EMITIDA);

                _pedidoRepository.Atualizar(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_NFE = true;
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }

        

        public IActionResult RegistrarRastreamento([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.CodigoRastreamento));

            if (ModelState.IsValid)
            {
                string codRastreamento = viewModel.CodigoRastreamento.Codigo;

                Pedido pedido = _pedidoRepository.ObterPedido(id);
                pedido.FreteCodRastreamento = codRastreamento;
                pedido.Situacao = PedidoSituacaoConstant.EM_TRANSPORTE;

                SalvarPedidoSituacao(id, codRastreamento, PedidoSituacaoConstant.EM_TRANSPORTE);

                _pedidoRepository.Atualizar(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_RASTREAMENTO = true;
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }


        public IActionResult RegistrarCancelamentoPedidoCartaoCredito([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.CartaoCredito));

            if (ModelState.IsValid)
            {
                viewModel.CartaoCredito.FormaPagamento = MetodoPagamentoConstant.CartaoCredito;

                Pedido pedido = _pedidoRepository.ObterPedido(id);
                _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);

                pedido.Situacao = PedidoSituacaoConstant.ESTORNO;

                SalvarPedidoSituacao(id, viewModel.CartaoCredito, PedidoSituacaoConstant.ESTORNO);

                _pedidoRepository.Atualizar(pedido);

                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_CARTAOCREDITO = true;
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }


        public IActionResult RegistrarCancelamentoPedidoBoletoBancario([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.BoletoBancario));

            if (ModelState.IsValid)
            {
                viewModel.BoletoBancario.FormaPagamento = MetodoPagamentoConstant.Boleto;

                Pedido pedido = _pedidoRepository.ObterPedido(id);
                _gerenciarPagarMe.EstornoBoletoBancario(pedido.TransactionId, viewModel.BoletoBancario);
                pedido.Situacao = PedidoSituacaoConstant.ESTORNO;

                SalvarPedidoSituacao(id, viewModel.BoletoBancario, PedidoSituacaoConstant.ESTORNO);

                _pedidoRepository.Atualizar(pedido);

                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_BOLETOBANCARIO = true;
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }



        public IActionResult RegistrarDevolucaoPedido([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.Devolucao));

            if (ModelState.IsValid)
            {
                Pedido pedido = _pedidoRepository.ObterPedido(id);
                pedido.Situacao = PedidoSituacaoConstant.DEVOLVER;

                SalvarPedidoSituacao(id, viewModel.Devolucao, PedidoSituacaoConstant.DEVOLVER);

                _pedidoRepository.Atualizar(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_DEVOLVER = true;
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarDevolucaoPedidoRejeicao([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.DevolucaoMotivoRejeicao));

            if (ModelState.IsValid)
            {
                Pedido pedido = _pedidoRepository.ObterPedido(id);
                pedido.Situacao = PedidoSituacaoConstant.DEVOLUCAO_REJEITADA;

                SalvarPedidoSituacao(id, viewModel.DevolucaoMotivoRejeicao, PedidoSituacaoConstant.DEVOLUCAO_REJEITADA);

                _pedidoRepository.Atualizar(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_DEVOLVER_REJEITAR = true;
            viewModel.Pedido = _pedidoRepository.ObterPedido(id);
            return View(nameof(Visualizar), viewModel);

        }

        public IActionResult RegistrarDevolucaoPedidoAprovadoCartaoCredito(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);
            if (pedido.Situacao == PedidoSituacaoConstant.DEVOLVER_ENTREGUE)
            {
                SalvarPedidoSituacao(id, "", PedidoSituacaoConstant.DEVOLUCAO_ACEITA);

                _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);

                SalvarPedidoSituacao(id, "", PedidoSituacaoConstant.DEVOLVER_ESTORNO);

                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                pedido.Situacao = PedidoSituacaoConstant.DEVOLVER_ESTORNO;
                _pedidoRepository.Atualizar(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            VisualizarViewModel viewModel = new VisualizarViewModel();
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }


        public IActionResult RegistrarDevolucaoPedidoAprovadoBoletoBancario([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.BoletoBancario), "BoletoBancario.Motivo");

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            if (ModelState.IsValid)
            {

                SalvarPedidoSituacao(id, "", PedidoSituacaoConstant.DEVOLUCAO_ACEITA);

                viewModel.BoletoBancario.FormaPagamento = MetodoPagamentoConstant.Boleto;

                _gerenciarPagarMe.EstornoBoletoBancario(pedido.TransactionId, viewModel.BoletoBancario);

                SalvarPedidoSituacao(id, viewModel.BoletoBancario, PedidoSituacaoConstant.DEVOLVER_ESTORNO);

                pedido.Situacao = PedidoSituacaoConstant.DEVOLVER_ESTORNO;
                _pedidoRepository.Atualizar(pedido);

                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }

            ViewBag.MODAL_DEVOLVERBOLETOBANCARIO = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }





        private void ValidarFormulario(string formularioParaValidar, params string[] removerFormularios)
        {
            //ModelState -> Validações.
            var propriedades = new VisualizarViewModel().GetType().GetProperties();

            foreach (var propriedade in propriedades)
            {
                //Pedido != NFE = true -> Sai da validação
                //NFE != NFE = false -> Continua na validação
                if(propriedade.Name != formularioParaValidar)
                {
                    ModelState.Remove(propriedade.Name);
                }
            }

            foreach (string removerFormulario in removerFormularios)
            {
                ModelState.Remove(removerFormulario);
            }
        }
        private void SalvarPedidoSituacao(int pedidoId, object dados, string situacao)
        {
            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            pedidoSituacao.Dados = JsonConvert.SerializeObject(dados);
            pedidoSituacao.PedidoId = pedidoId;
            pedidoSituacao.Situacao = situacao;
            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
        }
    }}