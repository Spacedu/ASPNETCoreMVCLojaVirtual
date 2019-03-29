using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Controllers
{
    public class ProdutoController : Controller
    {
        // Produto/ListagemCategoria/informatica
        [HttpGet]
        [Route("/Produto/Categoria/{slug}")]
        public IActionResult ListagemCategoria(string slug)
        {
            //TODO - Criar algoritmo recursivo que obtem uma lista com todas as categorias que devem ser utilizadas para apresentar o produto.

            //TODO - Adaptar o ProdutoRepository para receber uma lista de categorias e filtrar os produtos baseado nessa lista.

            return View();
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
