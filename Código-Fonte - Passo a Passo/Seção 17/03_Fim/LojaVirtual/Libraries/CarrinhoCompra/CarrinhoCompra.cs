using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.CarrinhoCompra
{
    public class CarrinhoCompra
    {
        private string Key = "Carrinho.Compras";
        private Cookie.Cookie _cookie;

        public CarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        /*
         * CRUD - Cadastrar, Read, Update, Delete
         * Adicionar Item, Remover Item, Alterar Quantidade
         */
        public void Cadastrar(Item item)
        {
            if( _cookie.Existe(Key) )
            {
                //Ler - Adicionar o Item no Carrinho Existente
            }
            else
            {
                //Criar o cookie com o item no Carrinho
            }
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
        public List<Item> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<Item>>(valor);
            }
            else
            {
                return new List<Item>();
            }
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

    public class Item
    {
        public int? Id { get; set; }
        public int? Quantidade { get; set; }
    }
}
