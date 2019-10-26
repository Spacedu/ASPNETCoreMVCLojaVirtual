using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoriaRepository;

        public ProdutoController(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult Index(int? pagina, string pesquisa)
        {
            var produtos = _produtoRepository.ObterTodosProdutos(pagina, pesquisa);
            return View(produtos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(a=> new SelectListItem(a.Nome, a.Id.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar(Produto produto) {
            if (ModelState.IsValid)
            {
                _produtoRepository.Cadastrar(produto);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(a => new SelectListItem(a.Nome, a.Id.ToString()));
            return View();
        }
    }
}