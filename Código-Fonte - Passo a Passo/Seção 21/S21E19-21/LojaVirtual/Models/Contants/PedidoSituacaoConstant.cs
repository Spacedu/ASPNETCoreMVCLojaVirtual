using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models.Contants
{
    public class PedidoSituacaoConstant
    {
        public const string PEDIDO_REALIZADO = "Pedido realizado";
        public const string PAGAMENTO_APROVADO = "Pagamento aprovado";
        public const string PAGAMENTO_REJEITADO = "Pagamento rejeitado";
        public const string PAGAMENTO_NAO_REALIZADO = "Pagamento não realizado (vencido)";

        public const string NF_EMITIDA = "NF Emitida";
        public const string EM_TRANSPORTE = "Em transporte";
        public const string ENTREGUE = "Entregue";
        public const string FINALIZADO = "Finalizado";
        public const string ESTORNO = "Estorno";
        

        public const string DEVOLVER = "Devolver (Em transporte)";
        public const string DEVOLVER_ENTREGUE = "Devolver (Entregue)";
        public const string DEVOLUCAO_ACEITA = "Devolução aceita";
        public const string DEVOLUCAO_REJEITADA = "Devolução rejeitada";
        public const string DEVOLVER_ESTORNO = "Estorno (Devolução de mercadoria)";


        public static string GetNames(string code)
        {
            foreach (var field in typeof(TipoFreteConstant).GetFields())
            {
                if ((string)field.GetValue(null) == code)
                    return field.Name.ToString();
            }
            return "";
        }
    }
}
