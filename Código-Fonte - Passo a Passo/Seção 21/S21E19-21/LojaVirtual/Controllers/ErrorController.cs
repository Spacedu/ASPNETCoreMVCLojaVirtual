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
        [Route("Error/{statusCode}")]
        public IActionResult ErroGenerico(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            ViewBag.Mensagem = ReasonPhrases.GetReasonPhrase(statusCode);

            return View();
        }

        public IActionResult Error500()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.Mensagem = exception.Error.Message;
            return View();
        }

        [Route("Error/503")]
        public IActionResult Error503()
        {
            return View();
        }


        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }


        [Route("Error/403")]
        public IActionResult Error403()
        {
            return View();
        }

    }
}