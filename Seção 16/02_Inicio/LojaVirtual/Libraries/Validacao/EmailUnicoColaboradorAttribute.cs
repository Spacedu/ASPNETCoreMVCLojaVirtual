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
            string Email = (value as string).Trim();

            IColaboradorRepository _colaboradorRepository = (IColaboradorRepository) validationContext.GetService(typeof(IColaboradorRepository));
            List<Colaborador> colaboradores = _colaboradorRepository.ObterColaboradorPorEmail(Email);

            Colaborador objColaborador = (Colaborador) validationContext.ObjectInstance;

            if(colaboradores.Count > 1)
            {
                return new ValidationResult("E-mail já existente!");
            }
            if(colaboradores.Count == 1 && objColaborador.Id != colaboradores[0].Id)
            {
                return new ValidationResult("E-mail já existente!");
            }


            return ValidationResult.Success;
        }
    }
}
