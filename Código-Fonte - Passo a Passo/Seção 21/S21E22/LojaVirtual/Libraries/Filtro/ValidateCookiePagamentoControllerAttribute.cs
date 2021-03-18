using LojaVirtual.Libraries.Lang;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LojaVirtual.Libraries.Filtro
{
    public class ValidateCookiePagamentoControllerAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var _cookie = (Cookie.Cookie)context.HttpContext.RequestServices.GetService(typeof(Cookie.Cookie));

            var tipoFreteUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            var valorFrete = _cookie.Consultar("Carrinho.ValorFrete", true);
            var carrinhoCompra = _cookie.Consultar("Carrinho.Compras", true);

            

            if (carrinhoCompra == null)
            {
                ((Controller)context.Controller).TempData["MSG_E"] = Mensagem.MSG_E010;
                context.Result = new RedirectToActionResult("Index", "CarrinhoCompra", null);
            }

            if(tipoFreteUsuario == null || valorFrete == null)
            {
                ((Controller)context.Controller).TempData["MSG_E"] = Mensagem.MSG_E009;
                context.Result = new RedirectToActionResult("EnderecoEntrega", "CarrinhoCompra", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
