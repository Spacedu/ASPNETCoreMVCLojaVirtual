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
        List<PedidoSituacaoStatus> timeline1 { get; set; }

        public PedidoSituacaoViewComponent()
        {
            timeline1 = new List<PedidoSituacaoStatus>();
            timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.AGUARDANDO_PAGAMENTO, Concluido = false });
            timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false });
            timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false });
            timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.EM_TRANSPORTE, Concluido = false });
            timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ENTREGUE, Concluido = false });
            timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.FINALIZADO, Concluido = false });


        }

        public async Task<IViewComponentResult> InvokeAsync(Pedido pedido)
        {
            List<PedidoSituacaoStatus> timeline = null;

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
                timeline = timeline1;

                foreach (var pedidoSituacao in pedido.PedidoSituacoes)
                {
                    var pedidoSituacaoTimeline = timeline1.Where(a => a.Situacao == pedidoSituacao.Situacao).FirstOrDefault();
                    pedidoSituacaoTimeline.Data = pedidoSituacao.Data;
                    pedidoSituacaoTimeline.Concluido = true;
                }
            }

            return View(timeline);
        }
    }
}
