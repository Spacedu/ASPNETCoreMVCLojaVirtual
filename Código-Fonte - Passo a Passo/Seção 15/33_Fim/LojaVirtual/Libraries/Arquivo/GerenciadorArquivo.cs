using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Arquivo
{
    public class GerenciadorArquivo
    {
        public static string CadastrarImagemProduto(IFormFile file)
        {
            var NomeArquivo = Path.GetFileName(file.FileName);
            var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", NomeArquivo);

            using(var stream = new FileStream(Caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("/uploads/temp", NomeArquivo).Replace("\\", "/");
        }

        public static bool ExcluirImagemProduto(string caminho)
        {
            string Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", caminho.TrimStart('/'));
            if (File.Exists(Caminho))
            {
                File.Delete(Caminho);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
