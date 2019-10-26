using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Cliente.Controllers
{
    public class ClienteController : Controller
    {
        [ClienteAutorizacao]
        public IActionResult Index()
        {
            return View();
        }

        [ClienteAutorizacao]
        public IActionResult Atualizar()
        {
            return View();
        }
    }
}