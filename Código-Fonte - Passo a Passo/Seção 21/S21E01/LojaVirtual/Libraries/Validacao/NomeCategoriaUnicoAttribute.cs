using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Validacao
{
    public class NomeCategoriaUnicoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ICategoriaRepository _categoriaRepository = (ICategoriaRepository)validationContext.GetService(typeof(ICategoriaRepository));
            Categoria categoria = (Categoria)validationContext.ObjectInstance;

            if(categoria.Id == 0)
            {
                Categoria categoriaDB = _categoriaRepository.ObterCategoriaPorNome(categoria.Nome);
                if(categoriaDB == null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            else
            {
                Categoria categoriaDB = _categoriaRepository.ObterCategoriaPorNome(categoria.Nome);

                if(categoriaDB == null)
                {
                    return ValidationResult.Success;
                }else if(categoriaDB.Id == categoria.Id)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
        }
    }
}
