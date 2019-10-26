using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Texto;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using Microsoft.Extensions.Configuration;
using PagarMe;

namespace LojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe
{
    public class GerenciarPagarMe
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private LoginCliente _loginCliente;

        public GerenciarPagarMe(IConfiguration configuration, LoginCliente loginCliente, IMapper mapper)
        {
            _configuration = configuration;
            _loginCliente = loginCliente;
            _mapper = mapper;
        }

        public Transaction GerarBoleto(decimal valor, List<ProdutoItem> produtos, EnderecoEntrega enderecoEntrega, ValorPrazoFrete valorFrete)
        {
            Cliente cliente = _loginCliente.GetCliente();

            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");
            PagarMeService.DefaultEncryptionKey = _configuration.GetValue<String>("Pagamento:PagarMe:EncryptionKey");
            int DaysExpire = _configuration.GetValue<int>("Pagamento:PagarMe:BoletoDiaExpiracao");

            Transaction transaction = new Transaction();

            transaction.Amount = Mascara.ConverterValorPagarMe(valor);
            transaction.PaymentMethod = PaymentMethod.Boleto;
            transaction.BoletoExpirationDate = DateTime.Now.AddDays(DaysExpire);

            transaction.Customer = new Customer
            {
                ExternalId = cliente.Id.ToString(),
                Name = cliente.Nome,
                Type = CustomerType.Individual,
                Country = "br",
                Email = cliente.Email,
                Documents = new[] {
                        new Document{
                            Type = DocumentType.Cpf,
                            Number = Mascara.Remover(cliente.CPF)
                        }
                    },
                PhoneNumbers = new string[]
                {
                        "+55" + Mascara.Remover( cliente.Telefone )
                },
                Birthday = cliente.Nascimento.ToString("yyyy-MM-dd")
            };

            var Today = DateTime.Now;
            var fee = Convert.ToDecimal(valorFrete.Valor);

            transaction.Shipping = new Shipping
            {
                Name = enderecoEntrega.Nome,
                Fee = Mascara.ConverterValorPagarMe(fee),
                DeliveryDate = Today.AddDays(_configuration.GetValue<int>("Frete:DiasNaEmpresa")).AddDays(valorFrete.Prazo).ToString("yyyy-MM-dd"),
                Expedited = false,
                Address = new Address()
                {
                    Country = "br",
                    State = enderecoEntrega.Estado,
                    City = enderecoEntrega.Cidade,
                    Neighborhood = enderecoEntrega.Bairro,
                    Street = enderecoEntrega.Endereco + " " + enderecoEntrega.Complemento,
                    StreetNumber = enderecoEntrega.Numero,
                    Zipcode = Mascara.Remover(enderecoEntrega.CEP)
                }
            };

            Item[] itens = new Item[produtos.Count];

            for (var i = 0; i < produtos.Count; i++)
            {
                var item = produtos[i];

                var itemA = new Item()
                {
                    Id = item.Id.ToString(),
                    Title = item.Nome,
                    Quantity = item.UnidadesPedidas,
                    Tangible = true,
                    UnitPrice = Mascara.ConverterValorPagarMe(item.Valor)
                };


                itens[i] = itemA;
            }

            transaction.Item = itens;

            transaction.Save();

            transaction.Customer.Gender = (cliente.Sexo == "M") ? Gender.Male : Gender.Female;
            return transaction;
        }


        public Transaction GerarPagCartaoCredito(CartaoCredito cartao, Parcelamento parcelamento, EnderecoEntrega enderecoEntrega, ValorPrazoFrete valorFrete, List<ProdutoItem> produtos)
        {
            Cliente cliente = _loginCliente.GetCliente();

            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");
            PagarMeService.DefaultEncryptionKey = _configuration.GetValue<String>("Pagamento:PagarMe:EncryptionKey");

            Card card = new Card();
            card.Number = cartao.NumeroCartao;
            card.HolderName = cartao.NomeNoCartao;
            card.ExpirationDate = cartao.VecimentoMM + cartao.VecimentoYY;
            card.Cvv = cartao.CodigoSeguranca;

            card.Save();

            Transaction transaction = new Transaction();
            transaction.PaymentMethod = PaymentMethod.CreditCard;
            /*
             * Transaction.postbackurl
             * - Parâmetro importante para que seu site seja informado sobre todas as mudanças de status ocorridas no Pagar.Me.
             * URL 1: https://pagarme.zendesk.com/hc/pt-br/articles/205973476-Quando-o-POSTback-%C3%A9-enviado-
             * URL 2: https://docs.pagar.me/v1/reference#criar-transacao
             */

            transaction.Card = new Card
            {
                Id = card.Id
            };

            transaction.Customer = new Customer
            {
                ExternalId = cliente.Id.ToString(),
                Name = cliente.Nome,
                Type = CustomerType.Individual,
                Country = "br",
                Email = cliente.Email,
                Documents = new[] {
                        new Document{
                            Type = DocumentType.Cpf,
                            Number = Mascara.Remover(cliente.CPF)
                        }
                    },
                PhoneNumbers = new string[]
                    {
                        "+55" + Mascara.Remover( cliente.Telefone )
                    },
                Birthday = cliente.Nascimento.ToString("yyyy-MM-dd")
            };

            transaction.Billing = new Billing
            {
                Name = cliente.Nome,
                Address = new Address()
                {
                    Country = "br",
                    State = cliente.Estado,
                    City = cliente.Cidade,
                    Neighborhood = cliente.Bairro,
                    Street = cliente.Endereco + " " + cliente.Complemento,
                    StreetNumber = cliente.Numero,
                    Zipcode = Mascara.Remover(cliente.CEP)
                }
            };

            var Today = DateTime.Now;
            var fee = Convert.ToDecimal(valorFrete.Valor);

            transaction.Shipping = new Shipping
            {
                Name = enderecoEntrega.Nome,
                Fee = Mascara.ConverterValorPagarMe(fee),
                DeliveryDate = Today.AddDays(_configuration.GetValue<int>("Frete:DiasNaEmpresa")).AddDays(valorFrete.Prazo).ToString("yyyy-MM-dd"),
                Expedited = false,
                Address = new Address()
                {
                    Country = "br",
                    State = enderecoEntrega.Estado,
                    City = enderecoEntrega.Cidade,
                    Neighborhood = enderecoEntrega.Bairro,
                    Street = enderecoEntrega.Endereco + " " + enderecoEntrega.Complemento,
                    StreetNumber = enderecoEntrega.Numero,
                    Zipcode = Mascara.Remover(enderecoEntrega.CEP)
                }
            };

            Item[] itens = new Item[produtos.Count];

            for (var i = 0; i < produtos.Count; i++)
            {
                var item = produtos[i];

                var itemA = new Item()
                {
                    Id = item.Id.ToString(),
                    Title = item.Nome,
                    Quantity = item.UnidadesPedidas,
                    Tangible = true,
                    UnitPrice = Mascara.ConverterValorPagarMe(item.Valor)
                };


                itens[i] = itemA;
            }

            transaction.Item = itens;
            transaction.Amount = Mascara.ConverterValorPagarMe(parcelamento.Valor);
            transaction.Installments = parcelamento.Numero;

            transaction.Save();

            transaction.Customer.Gender = (cliente.Sexo == "M") ? Gender.Male : Gender.Female;
            return transaction;
        }


        public List<Parcelamento> CalcularPagamentoParcelado(decimal valor)
        {
            List<Parcelamento> lista = new List<Parcelamento>();

            int maxParcelamento = _configuration.GetValue<int>("Pagamento:PagarMe:MaxParcelas");
            int parcelaPagaVendedor = _configuration.GetValue<int>("Pagamento:PagarMe:ParcelaPagaVendedor");
            decimal juros = _configuration.GetValue<decimal>("Pagamento:PagarMe:Juros");

            for (int i = 1; i <= maxParcelamento; i++)
            {
                Parcelamento parcelamento = new Parcelamento();
                parcelamento.Numero = i;

                if (i > parcelaPagaVendedor)
                {
                    //Juros - i = (4-3 - parcelaPagaVendedor) + 5%
                    int quantidadeParcelasComJuros = i - parcelaPagaVendedor;
                    decimal valorDoJuros = valor * juros / 100;

                    parcelamento.Valor = quantidadeParcelasComJuros * valorDoJuros + valor;
                    parcelamento.ValorPorParcela = parcelamento.Valor / parcelamento.Numero;
                    parcelamento.Juros = true;

                }
                else
                {
                    parcelamento.Valor = valor;
                    parcelamento.ValorPorParcela = parcelamento.Valor / parcelamento.Numero;
                    parcelamento.Juros = false;
                }
                lista.Add(parcelamento);
            }

            return lista;
        }

        public Transaction ObterTransacao(string transactionId)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");

            return PagarMeService.GetDefaultService().Transactions.Find(transactionId);
        }
        public Transaction EstornoCartaoCredito(string transactionId)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(transactionId);

            transaction.Refund();

            return transaction;
        }


        public Transaction EstornoBoletoBancario(string transactionId, DadosCancelamentoBoleto boletoBancario)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(transactionId);
            var bankAccount = _mapper.Map<DadosCancelamentoBoleto, BankAccount>(boletoBancario);

            transaction.Refund(bankAccount);

            return transaction;
        }
    }

}
