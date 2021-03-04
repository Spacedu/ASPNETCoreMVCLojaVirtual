using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class Parcelamento
    {
        public int Numero { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPorParcela { get; set; }
        public bool Juros { get; set; }
    }
}
