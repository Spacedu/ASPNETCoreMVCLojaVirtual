using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models.ViewModels.Pagamento
{
    public class IndexViewModel
    {
        public CartaoCredito CartaoCredito { get; set; }
        public Parcelamento Parcelamento { get; set; }
    }
}
