using PagarMe;
using PagarMe.Base;
using PagarMe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models
{
    public class TransacaoPagarMe
    {
        public string Id { get; set; }
        public int? Installments { get; set; }
        public int Cost { get; set; }
        public string CardHolderName { get; set; }
        public string CardCvv { get; set; }
        public string CardLastDigits { get; set; }
        public string CardFirstDigits { get; set; }
        public string CardEmvResponse { get; set; }
        public string PostbackUrl { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public float? AntifraudScore { get; set; }
        public string BoletoUrl { get; set; }
        public string BoletoInstructions { get; set; }
        public DateTime? BoletoExpirationDate { get; set; }
        public string BoletoBarcode { get; set; }
        public string Referer { get; set; }
        public string IP { get; set; }
        public bool? ShouldCapture { get; set; }
        public bool? Async { get; set; }
        public string LocalTime { get; set; }
        public AbstractModel Metadata { get; set; }
        public Billing Billing { get; set; }
        public Shipping Shipping { get; set; }
        public Item[] Item { get; set; }
        public ModelCollection<Event> Events { get; }
        public ModelCollection<Payable> Payables { get; }
        public int Amount { get; set; }
        public string Nsu { get; }
        public string Tid { get; }
        public string RefuseReason { get; }
        public Card Card { get; set; }
        public Customer Customer { get; set; }
        public ModelCollection<AntifraudAnalysis> AntifraudAnalysis { get; }
        public Phone Phone { get; set; }
        public TransactionStatus Status { get; set; }
        public int AuthorizationAmount { get; set; }
        public int PaidAmount { get; set; }
        public Address Address { get; set; }
        public string CardHash { get; set; }
        public int RefundedAmount { get; set; }
        public string SoftDescriptor { get; set; }
        public string AuthorizationCode { get; set; }
        public string AcquirerName { get; set; }
        public ModelCollection<Postback> Postbacks { get; }
        public string StatusReason { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardNumber { get; set; }
        public string AcquirerResponseCode { get; set; }
        protected string Endpoint { get; }
        public DateTime? DateCreated { get; set; }
    }
}
