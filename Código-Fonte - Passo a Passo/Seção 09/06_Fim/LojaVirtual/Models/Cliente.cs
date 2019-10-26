using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class Cliente
    {
        /* PK */
        public int Id { get; set; }

        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public string Sexo { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }

    }
}
