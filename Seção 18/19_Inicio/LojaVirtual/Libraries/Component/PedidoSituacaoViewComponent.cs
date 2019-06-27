using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Component
{
    public class PedidoSituacaoViewComponent : ViewComponent
    {
        List<PedidoSituacaoStatus> timeline1
        {
            get
            {
                var lista = new List<PedidoSituacaoStatus>();
                lista.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.AGUARDANDO_PAGAMENTO, Concluido = false });
                lista.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false });
                lista.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false });
                lista.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.EM_TRANSPORTE, Concluido = false });
                lista.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ENTREGUE, Concluido = false });
                lista.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.FINALIZADO, Concluido = false });

                return lista;
            }
        }

        public PedidoSituacaoViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(Pedido pedido)
        {
            var listaStatusTimeline1 = new List<string>() {
                PedidoSituacaoConstant.AGUARDANDO_PAGAMENTO,
                PedidoSituacaoConstant.PAGAMENTO_APROVADO,
                PedidoSituacaoConstant.NF_EMITIDA,
                PedidoSituacaoConstant.EM_TRANSPORTE,
                PedidoSituacaoConstant.ENTREGUE,
                PedidoSituacaoConstant.FINALIZADO,
            };

            if( listaStatusTimeline1.Contains(pedido.Situacao))
            {
                foreach (var pedidoSituacao in pedido.PedidoSituacoes)
                {
                    var pedidoSituacaoTimeline = timeline1.Where(a => a.Situacao == pedidoSituacao.Situacao).FirstOrDefault();
                    pedidoSituacaoTimeline.Data = pedidoSituacao.Data;
                    pedidoSituacaoTimeline.Concluido = true;
                }
            }

            return View(listaStatusTimeline1);
        }
    }
}
