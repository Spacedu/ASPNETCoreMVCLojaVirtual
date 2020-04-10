using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class Pedido
    {
        //PK
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public string TransactionId { get; set; } //PagarMe - Transaction -> ID.

        //Frete
        public string FreteEmpresa { get; set; } //ECT - Correios
        public string FreteCodRastreamento { get; set; }

        public string FormaPagamento { get; set; } //Boleto-Cartão Credito
        public decimal ValorTotal { get; set; }
        public string DadosTransaction { get; set; } //Transaction - JSON
        public string DadosProdutos { get; set; } //ProdutoItem - JSON

        public DateTime DataRegistro { get; set; }
        public string Situacao { get; set; }

        //URL - Com Site da Receita - Nota Fiscal
        public string NFE { get; set; }
        
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("PedidoId")]
        public virtual ICollection<PedidoSituacao> PedidoSituacoes { get; set; }
    }
}
