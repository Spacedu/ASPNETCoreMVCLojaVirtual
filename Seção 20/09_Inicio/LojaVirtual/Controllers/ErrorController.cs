using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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

        [Route("Error/{statusCode}")]
        public IActionResult ErroGenerico(int statusCode)
        {
            ViewBag.Mensagem = string.Format("Código de erro: {0} - Mensagem: {1}", statusCode, ReasonPhrases.GetReasonPhrase(statusCode));
            return View("Error500");
        }
    }
}