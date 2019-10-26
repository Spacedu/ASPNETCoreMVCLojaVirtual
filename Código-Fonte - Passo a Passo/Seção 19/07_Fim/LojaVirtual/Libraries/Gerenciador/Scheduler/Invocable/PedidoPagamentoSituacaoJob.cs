using Coravel.Invocable;
using LojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using LojaVirtual.Repositories.Contracts;
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

        public PedidoPagamentoSituacaoJob(GerenciarPagarMe gerenciarPagarMe, IPedidoRepository pedidoRepository)
        {
            _gerenciarPagarMe = gerenciarPagarMe;
            _pedidoRepository = pedidoRepository;
        }

        public Task Invoke()
        {
            var pedidosRealizados = _pedidoRepository.ObterTodosPedidosRealizados();
            foreach (var pedido in pedidosRealizados)
            {
                var transaction = _gerenciarPagarMe.ObterTransacao(pedido.TransactionId);

            }


            /*if(transaction.Status == TransactionStatus.Authorized || transaction.Status == TransactionStatus.Paid)
            {
                transaction.DateUpdated;
            }
            */

            Debug.WriteLine("--PedidoPagamentoSituacaoJob - Executado!--");

            return Task.CompletedTask;
        }
    }
}
