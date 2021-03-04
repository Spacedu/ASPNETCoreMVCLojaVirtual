using Coravel.Invocable;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LojaVirtual.Libraries.Gerenciador.Scheduler.Invocable
{
    public class PedidoEntregueJob : IInvocable
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private ILogger<PedidoEntregueJob> _logger;

        public PedidoEntregueJob(ILogger<PedidoEntregueJob> logger, IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _logger = logger;
        }

        public Task Invoke()
        {
            _logger.LogInformation("> PedidoEntregueJob: Iniciando");
            var pedidos = _pedidoRepository.ObterTodosPedidosPorSituacao(PedidoSituacaoConstant.EM_TRANSPORTE);
            foreach (var pedido in pedidos)
            {
                var result = new Correios.NET.Services().GetPackageTracking(pedido.FreteCodRastreamento);

                if (result.IsDelivered)
                {
                    PedidoSituacao pedidoSituacao = new PedidoSituacao();
                    pedidoSituacao.PedidoId = pedido.Id;
                    pedidoSituacao.Situacao = PedidoSituacaoConstant.ENTREGUE;
                    pedidoSituacao.Data = result.DeliveryDate.Value;
                    pedidoSituacao.Dados = JsonConvert.SerializeObject(result);

                    _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                    pedido.Situacao = PedidoSituacaoConstant.ENTREGUE;
                    _pedidoRepository.Atualizar(pedido);
                }

            }
            _logger.LogInformation("> PedidoEntregueJob: Finalizado");
            return Task.CompletedTask;
        }
    }
}
