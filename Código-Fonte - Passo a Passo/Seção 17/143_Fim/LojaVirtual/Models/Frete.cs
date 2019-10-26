using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class Frete
    {
        public int CEP { get; set; }
        //CodCarrinho - HashCode MD5.
        public string CodCarrinho { get; set; }
        public List<ValorPrazoFrete> ListaValores { get; set; }
    }
}
