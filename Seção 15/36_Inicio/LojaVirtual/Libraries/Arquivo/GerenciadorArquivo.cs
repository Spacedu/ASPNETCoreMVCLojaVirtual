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

        public static List<string> MoverImagensProduto(List<string> ListaCaminhoTemp, string ProdutoId)
        {
            /*
             * Criar a Pasta do Produto
             */
            var CaminhoDefinitivoPastaProduto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", ProdutoId);
            if (!Directory.Exists(CaminhoDefinitivoPastaProduto))
            {
                Directory.CreateDirectory(CaminhoDefinitivoPastaProduto);
            }

            /*
             * Mover a Imagem da Pasta Temp para a pasta definitiva
             */
            List<string> ListaCaminhoDef = new List<string>();
            foreach (var CaminhoTemp in ListaCaminhoTemp)
            {
                if(string.IsNullOrEmpty(CaminhoTemp))
                {
                    // /uploads/temp/mouse-cosair.jpg
                    var NomeArquivo = Path.GetFileName( CaminhoTemp );

                    var CaminhoAbsolutoTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", CaminhoTemp);
                    var CaminhoAbsolutoDef = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", ProdutoId, NomeArquivo);

                    if(File.Exists(CaminhoAbsolutoTemp))
                    {
                        File.Copy(CaminhoAbsolutoTemp, CaminhoAbsolutoDef);
                        if (File.Exists(CaminhoAbsolutoDef))
                        {
                            File.Delete(CaminhoAbsolutoTemp);
                        }

                        ListaCaminhoDef.Add(Path.Combine("/uploads", ProdutoId, NomeArquivo).Replace("\\", "/") );
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return ListaCaminhoDef;
        }
    }
}
