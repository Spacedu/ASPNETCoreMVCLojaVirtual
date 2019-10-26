using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LojaVirtual.Libraries.Filtro
{
    /*
     * Tipos de Filtros:
     * - Autorização
     * - Recurso
     * - Ação
     * - Exceção
     * - Resultado
     */
    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginCliente _loginCliente;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));
            Cliente cliente = _loginCliente.GetCliente();
            if (cliente == null)
            {
                //TODO - Implementar página HTML bonita para acesso negado/ERRO4XX/ERRO5XX
                context.Result = new ContentResult() { Content = "Acesso negado." };
            }
        }
    }
}
