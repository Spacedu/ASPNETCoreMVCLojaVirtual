using LojaVirtual.Libraries.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class DadosCancelamento
    {
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string Motivo { get; set; }
        public string FormaPagamento { get; set; }

        public string BancoCodigo { get; set; }
        public string Agencia { get; set; }
        public string AgenciaDV { get; set; }
        public string Conta { get; set; }
        public string ContaDV { get; set; }
        //TODO - Validar CPF
        public string CPF { get; set; }

        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        public string Nome { get; set; }
        public string TipoConta { get; set; }
    }
}
