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
        public const string NF_EMITIDA = "NF Emitida";
        public const string EM_TRANSPORTE = "Em transporte";
        public const string ENTREGUE = "Entregue";
        public const string FINALIZADO = "Finalizado"; //TODO - Criar Processamento de 7 dias da entrega - finalizar o pedido.
        public const string EM_CANCELAMENTO = "Em cancelamento"; //TODO - Somente funcionário faz o cancelamento.
        public const string EM_ANALISE = "Em análise"; //TODO - 2 dias úteis em análise.
        public const string CANCELAMENTO_ACEITO = "Cancelamento aceito"; //TODO - 2 dias úteis em análise.
        public const string CANCELAMENTO_REJEITADO = "Cancelamento rejeitado"; //TODO - 2 dias úteis em análise.
        public const string ESTORNO = "Estorno"; //TODO - Cancelamento aceito.

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
