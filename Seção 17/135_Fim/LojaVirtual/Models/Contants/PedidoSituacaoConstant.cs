using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models.Contants
{
    public class PedidoSituacaoConstant
    {
        public const string AGUARDANDO_PAGAMENTO = "04014";
        public const string PAGAMENTO_APROVADO = "40215";
        public const string PAGAMENTO_REJEITADO = "04510";
        public const string NF_EMITIDA = "04510";
        public const string EM_TRANSPORTE = "04510";
        public const string ENTREGUE = "04510";
        public const string FINALIZADO = "04510"; //TODO - Criar Processamento de 7 dias da entrega - finalizar o pedido.
        public const string EM_CANCELAMENTO = "04510"; //TODO - Somente funcionário faz o cancelamento.
        public const string EM_ANALISE = "04510"; //TODO - 2 dias úteis em análise.
        public const string CANCELAMENTO_ACEITO = "04510"; //TODO - 2 dias úteis em análise.
        public const string CANCELAMENTO_REJEITADO = "04510"; //TODO - 2 dias úteis em análise.
        public const string ESTORNO = "04510"; //TODO - Cancelamento aceito.

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
