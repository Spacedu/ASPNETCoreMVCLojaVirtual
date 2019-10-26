using LojaVirtual.Models.ProdutoAgregador;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class TransactionProduto
    {
        public TransacaoPagarMe Transaction { get; set; }
        public List<ProdutoItem> Produtos { get; set; }
    }
}
