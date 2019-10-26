using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRespository;
        public ColaboradorController(IColaboradorRepository colaboradorRespository)
        {
            _colaboradorRespository = colaboradorRespository;
        }

        public IActionResult Index(int? pagina)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Models.Colaborador colaborador)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm]Models.Colaborador colaborador, int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            return View();
        }
    }
}