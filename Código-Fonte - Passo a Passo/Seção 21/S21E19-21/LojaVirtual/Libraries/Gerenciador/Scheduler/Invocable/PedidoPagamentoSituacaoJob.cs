using AutoMapper;
using Coravel.Invocable;
using LojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using LojaVirtual.Libraries.Json.Resolver;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Gerenciador.Scheduler.Invocable
{
    public class PedidoPagamentoSituacaoJob : IInvocable
    {
        private GerenciarPagarMe _gerenciarPagarMe;
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IProdutoRepository _produtoRepository;
        private ILogger<PedidoPagamentoSituacaoJob> _logger;

        public PedidoPagamentoSituacaoJob(ILogger<PedidoPagamentoSituacaoJob> logger, GerenciarPagarMe gerenciarPagarMe, IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository, IMapper mapper, IConfiguration configuration, IProdutoRepository produtoRepository)
        {
            _gerenciarPagarMe = gerenciarPagarMe;
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _configuration = configuration;
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        public Task Invoke()
        {
            _logger.LogInformation("> PedidoPagamentoSituacaoJob: Iniciando");
            var pedidosRealizados = _pedidoRepository.ObterTodosPedidosPorSituacao(PedidoSituacaoConstant.PEDIDO_REALIZADO);

            foreach (var pedido in pedidosRealizados)
            {
                string situacao = null;
                var transaction = _gerenciarPagarMe.ObterTransacao(pedido.TransactionId);

                int toleranciaDias = _configuration.GetValue<int>("Pagamento:PagarMe:BoletoDiaExpiracao") + _configuration.GetValue<int>("Pagamento:PagarMe:BoletoDiaToleranciaVencido");
                if (transaction.Status == TransactionStatus.WaitingPayment && transaction.PaymentMethod == PaymentMethod.Boleto && DateTime.Now > pedido.DataRegistro.AddDays(toleranciaDias) )
                {
                    situacao = PedidoSituacaoConstant.PAGAMENTO_NAO_REALIZADO;
                    _produtoRepository.DevolverProdutoAoEstoque(pedido);
                }

                if (transaction.Status == TransactionStatus.Refused)
                {
                    situacao = PedidoSituacaoConstant.PAGAMENTO_REJEITADO;
                    _produtoRepository.DevolverProdutoAoEstoque(pedido);
                }

                if (transaction.Status == TransactionStatus.Authorized || transaction.Status == TransactionStatus.Paid)
                {
                    situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO;
                }

                if (transaction.Status == TransactionStatus.Refunded)
                {
                    situacao = PedidoSituacaoConstant.ESTORNO;
                    _produtoRepository.DevolverProdutoAoEstoque(pedido);
                }

                if(situacao != null)
                {
                    TransacaoPagarMe transacaoPagarMe = _mapper.Map<Transaction, TransacaoPagarMe>(transaction);
                    transacaoPagarMe.Customer.Gender = (pedido.Cliente.Sexo == "M") ? Gender.Male : Gender.Female;

                    PedidoSituacao pedidoSituacao = new PedidoSituacao();
                    pedidoSituacao.PedidoId = pedido.Id;
                    pedidoSituacao.Situacao = situacao;
                    pedidoSituacao.Data = transaction.DateUpdated.Value.ToLocalTime();
                    pedidoSituacao.Dados = JsonConvert.SerializeObject(transacaoPagarMe);

                    _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                    pedido.Situacao = situacao;
                    _pedidoRepository.Atualizar(pedido);
                }                
            }

            _logger.LogInformation("> PedidoPagamentoSituacaoJob: Finalizado");

            return Task.CompletedTask;
        }

    }
}
