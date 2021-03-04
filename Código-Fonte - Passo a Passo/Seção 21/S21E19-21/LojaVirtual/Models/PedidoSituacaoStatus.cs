using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class PedidoSituacaoStatus
    {
        public string Situacao { get; set; }
        public DateTime? Data { get; set; }
        public bool Concluido { get; set; }
        public string Cor { get; set; }
    }
}
