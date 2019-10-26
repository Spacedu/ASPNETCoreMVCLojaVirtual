using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class DadosCancelamento
    {
        public string Motivo { get; set; }

        public string bank_code { get; set; }
        public string agencia { get; set; }
        public string agencia_dv { get; set; }
        public string conta { get; set; }
        public string conta_dv { get; set; }
        public string document_number { get; set; }
        public string legal_name { get; set; }
        public string type { get; set; }
    }
}
