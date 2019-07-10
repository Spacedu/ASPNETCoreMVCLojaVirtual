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
        List<PedidoSituacaoStatus> Timeline1 { get; set; }
        List<string> StatusTimeline1 = new List<string>() {
                PedidoSituacaoConstant.PEDIDO_REALIZADO,
                PedidoSituacaoConstant.PAGAMENTO_APROVADO,
                PedidoSituacaoConstant.NF_EMITIDA,
                PedidoSituacaoConstant.EM_TRANSPORTE,
                PedidoSituacaoConstant.ENTREGUE,
                PedidoSituacaoConstant.FINALIZADO,
            };

        List<PedidoSituacaoStatus> Timeline2 { get; set; }
        List<string> StatusTimeline2 = new List<string>() {
                PedidoSituacaoConstant.PAGAMENTO_NAO_REALIZADO
            };

        List<PedidoSituacaoStatus> Timeline3 { get; set; }
        List<string> StatusTimeline3 = new List<string>() {
                PedidoSituacaoConstant.ESTORNO
            };

        public PedidoSituacaoViewComponent()
        {
            Timeline1 = new List<PedidoSituacaoStatus>();
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.EM_TRANSPORTE, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ENTREGUE, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.FINALIZADO, Concluido = false, Cor = "complete" });

            Timeline2 = new List<PedidoSituacaoStatus>();
            Timeline2.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline2.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_NAO_REALIZADO, Concluido = false, Cor = "complete-red" });

            Timeline3 = new List<PedidoSituacaoStatus>();
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false, Cor = "complete" });
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false, Cor = "complete" });
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ESTORNO, Concluido = false, Cor = "complete-red" });
        }

        public async Task<IViewComponentResult> InvokeAsync(Pedido pedido)
        {
            List<PedidoSituacaoStatus> timeline = null;

            if (StatusTimeline1.Contains(pedido.Situacao))
            {
                timeline = Timeline1;
            }

            if (StatusTimeline2.Contains(pedido.Situacao))
            {
                timeline = Timeline2;
            }

            if (StatusTimeline3.Contains(pedido.Situacao))
            {
                timeline = Timeline3;

                var nfe = pedido.PedidoSituacoes.Where(a => a.Situacao == PedidoSituacaoConstant.NF_EMITIDA).FirstOrDefault();
                if(nfe == null)
                {
                    timeline.Remove( timeline.FirstOrDefault(a => a.Situacao == PedidoSituacaoConstant.NF_EMITIDA) );
                }
            }

            if (timeline != null)
            {
                foreach (var pedidoSituacao in pedido.PedidoSituacoes)
                {
                    var pedidoSituacaoTimeline = timeline.Where(a => a.Situacao == pedidoSituacao.Situacao).FirstOrDefault();
                    pedidoSituacaoTimeline.Data = pedidoSituacao.Data;
                    pedidoSituacaoTimeline.Concluido = true;
                }
            }


            return View(timeline);
        }
    }
}
