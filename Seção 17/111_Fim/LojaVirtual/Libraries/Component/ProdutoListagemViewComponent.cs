using LojaVirtual.Models;
using LojaVirtual.Models.ViewModels;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Component
{
    public class ProdutoListagemViewComponent : ViewComponent
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoriaRepository;
        public ProdutoListagemViewComponent(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? pagina = 1;
            string pesquisa = "";
            string ordenacao = "A";
            IEnumerable<Categoria> categorias = null;

            if (HttpContext.Request.Query.ContainsKey("pagina"))
            {
                pagina = int.Parse(HttpContext.Request.Query["pagina"]);
            }
            if (HttpContext.Request.Query.ContainsKey("pesquisa"))
            {
                pesquisa = HttpContext.Request.Query["pesquisa"].ToString();
            }
            if (HttpContext.Request.Query.ContainsKey("ordenacao"))
            {
                ordenacao = HttpContext.Request.Query["ordenacao"];
            }
            if (ViewContext.RouteData.Values.ContainsKey("slug"))
            {
                string slug = ViewContext.RouteData.Values["slug"].ToString();
                Categoria CategoriaPrincipal = _categoriaRepository.ObterCategoria(slug);
                categorias = _categoriaRepository.ObterCategoriasRecursivas(CategoriaPrincipal);
            }
            var viewModel = new ProdutoListagemViewModel() { lista = _produtoRepository.ObterTodosProdutos(pagina, pesquisa, ordenacao, categorias) };
            
            return View(viewModel);
        }
    }
}
