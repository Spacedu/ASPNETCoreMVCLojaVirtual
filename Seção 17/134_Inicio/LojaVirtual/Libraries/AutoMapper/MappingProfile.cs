using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LojaVirtual.Libraries.Texto;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using Newtonsoft.Json;
using PagarMe;

namespace LojaVirtual.Libraries.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoItem>();
            CreateMap<Cliente, EnderecoEntrega>()
                .ForMember(dest=>dest.Id, opt=>opt.MapFrom(orig=>0))
                .ForMember(dest=>dest.Nome, opt=>opt.MapFrom(
                    orig=>
                    string.Format("Endereço do cliente ({0})", orig.Nome)
                ));


            CreateMap<Transaction, Pedido>()
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(orig => int.Parse(orig.Customer.Id)))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.FreteEmpresa, opt => opt.MapFrom(orig => "ECT - Correios"))
                .ForMember(dest => dest.FormaPagamento, opt => opt.MapFrom(orig => (orig.PaymentMethod == 0) ? "Cartão de Crédito" : "Boleto"))
                .ForMember(dest => dest.DadosTransaction, opt => opt.MapFrom(orig => JsonConvert.SerializeObject(orig)))
                .ForMember(dest => dest.DataRegistro, opt => opt.MapFrom(orig => DateTime.Now))
                .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(orig => Mascara.ConverterPagarMeIntToDecimal( orig.Amount )));
        }
    }
}
