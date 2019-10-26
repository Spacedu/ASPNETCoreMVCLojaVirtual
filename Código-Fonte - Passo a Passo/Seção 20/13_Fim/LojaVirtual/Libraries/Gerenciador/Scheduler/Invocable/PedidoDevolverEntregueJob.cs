using Coravel.Invocable;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Gerenciador.Scheduler.Invocable
{
    public class PedidoDevolverEntregueJob : IInvocable
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;

        public PedidoDevolverEntregueJob(IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
        }

        public Task Invoke()
        {
            var pedidos = _pedidoRepository.ObterTodosPedidosPorSituacao(PedidoSituacaoConstant.DEVOLVER);
            foreach (var pedido in pedidos)
            {
                var result = new Correios.NET.Services().GetPackageTracking(pedido.FreteCodRastreamento);

                if (result.IsDelivered)
                {
                    PedidoSituacao pedidoSituacao = new PedidoSituacao();
                    pedidoSituacao.PedidoId = pedido.Id;
                    pedidoSituacao.Situacao = PedidoSituacaoConstant.DEVOLVER_ENTREGUE;
                    pedidoSituacao.Data = result.DeliveryDate.Value;
                    pedidoSituacao.Dados = JsonConvert.SerializeObject(result);

                    _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                    pedido.Situacao = PedidoSituacaoConstant.DEVOLVER_ENTREGUE;
                    _pedidoRepository.Atualizar(pedido);
                }

            }

            return Task.CompletedTask;
        }
    }
}
