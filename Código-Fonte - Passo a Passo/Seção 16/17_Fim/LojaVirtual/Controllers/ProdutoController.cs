using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Controllers
{
    public class ProdutoController : Controller
    {
        private ICategoriaRepository _categoriaRepository;
        private IProdutoRepository _produtoRepository;

        public ProdutoController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }
        
        [HttpGet]
        [Route("/Produto/Categoria/{slug}")]
        public IActionResult ListagemCategoria(string slug)
        {
            //TODO - Criar algoritmo recursivo que obtem uma lista com todas as categorias que devem ser utilizadas para apresentar o produto.
            Categoria CategoriaPrincipal = _categoriaRepository.ObterCategoria(slug);
            List<Categoria> lista = GetCategorias(_categoriaRepository.ObterTodasCategorias().ToList(), CategoriaPrincipal);

            //TODO - Adaptar o ProdutoRepository para receber uma lista de categorias e filtrar os produtos baseado nessa lista.

            return View();
        }

        private List<Categoria> lista = new List<Categoria>();
        private List<Categoria> GetCategorias(List<Categoria> categorias, Categoria CategoriaPrincipal)
        {
            var ListaCategoriaFilho = categorias.Where(a => a.CategoriaPaiId == CategoriaPrincipal.Id);
            if (ListaCategoriaFilho.Count() > 0)
            {
                lista.AddRange(ListaCategoriaFilho.ToList());
                foreach (var categoria in ListaCategoriaFilho)
                {
                    GetCategorias(categorias, categoria);
                }
            }

            return lista;
        }


        /* ---------------------------------------------------------------------- */


        /*
         * ActionResult
         * IActionResult
         */
        public ActionResult Visualizar()
        {
            Produto produto = GetProduto();

            return View(produto);
            //return new ContentResult() { Content = "<h3>Produto -> Visualizar<h3>", ContentType = "text/html" };
        }

        private Produto GetProduto()
        {
            return new Produto()
            {
                Id = 1,
                Nome = "Xbox One X",
                Descricao = "Jogue em 4k",
                Valor = 2000.00M
            };
        }
    }
}
