using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models.ProdutoAgregador
{
    public class ProdutoItem : Produto
    {
        /*
         * Armazena a quantidade de produtos que o usuário pretende comprar deste item.
         */
        public int QuantidadeProdutoCarrinho { get; set; }
    }
}
