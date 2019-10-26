using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Validacao
{
    public class EmailUnicoColaboradorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //TODO - Fazer verificação
            string Email = value as string;

            IColaboradorRepository _colaboradorRepository = (IColaboradorRepository) validationContext.GetService(typeof(IColaboradorRepository));
            List<Colaborador> colaboradores = _colaboradorRepository.ObterColaboradorPorEmail(Email);



            return base.IsValid(value, validationContext);
        }
    }
}
