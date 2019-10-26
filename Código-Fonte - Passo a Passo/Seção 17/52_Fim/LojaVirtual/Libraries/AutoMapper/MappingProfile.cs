using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LojaVirtual.Models.ProdutoAgregador;

namespace LojaVirtual.Libraries.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoItem>();
        }
    }
}
