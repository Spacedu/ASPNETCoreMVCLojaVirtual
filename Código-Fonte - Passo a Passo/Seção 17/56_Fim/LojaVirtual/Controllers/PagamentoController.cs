using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class PagamentoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}