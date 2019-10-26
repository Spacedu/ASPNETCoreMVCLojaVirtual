using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Texto
{
    public class Mascara
    {
        public static string Remover(string valor)
        {
            return valor.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("R$", "").Replace(",", "").Replace(" ", "");
        }
        /*
         * PagarMe
         * - O PagarMe recebe o valor no seguinte formato: 3310, que representa R$ 33,10
         */
        
        public static int ConverterValorPagarMe(decimal valor)
        {
            string valorString = valor.ToString("C");
            valorString = Remover(valorString);
            int valorInt = int.Parse(valorString);

            return valorInt;
        }
    }
}
