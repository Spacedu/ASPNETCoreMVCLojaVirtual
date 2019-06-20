using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models.Contants
{
    public class TipoFreteConstant
    {
        public const string SEDEX = "04014";
        public const string SEDEX10 = "40215";
        public const string PAC = "04510";

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
