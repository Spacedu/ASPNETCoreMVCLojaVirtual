using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error500()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.Mensagem = exception.Error.Message;
            return View();
        }
    }
}