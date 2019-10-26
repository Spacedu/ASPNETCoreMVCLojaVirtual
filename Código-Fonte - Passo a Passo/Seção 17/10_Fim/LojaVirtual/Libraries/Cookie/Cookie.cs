using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Cookie
{
    public class Cookie
    {
        private IHttpContextAccessor _context;
        public Cookie(IHttpContextAccessor context)
        {
            _context = context;
        }

        /*
         * CRUD - Cadastrar/Atualizar/Consultar/Remover - RemoverTodos/Exist
         */
        public void Cadastrar(string Key, string Valor)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateTime.Now.AddDays(7);

            _context.HttpContext.Response.Cookies.Append(Key, Valor, Options);
        }
        public void Atualizar(string Key, string Valor)
        {
            if (Existe(Key))
            {
                Remover(Key);
            }
            Cadastrar(Key, Valor);
        }
        public void Remover(string Key)
        {
            _context.HttpContext.Response.Cookies.Delete(Key);
        }
        public string Consultar(string Key)
        {
            return _context.HttpContext.Request.Cookies[Key];
        }


        public bool Existe(string Key)
        {
            if (_context.HttpContext.Request.Cookies[Key] == null)
            {
                return false;
            }

            return true;
        }
        public void RemoverTodos()
        {
            var ListaCookie = _context.HttpContext.Request.Cookies.ToList();
            foreach (var cookie in ListaCookie)
            {
                Remover(cookie.Key);
            }
            
        }
    }
}
