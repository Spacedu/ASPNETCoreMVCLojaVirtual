using LojaVirtual.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.CarrinhoCompra
{
    public class CookieFrete
    {
        private string Key = "Carrinho.ValorFrete";
        private Cookie.Cookie _cookie;

        public CookieFrete(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        /*
         * CRUD - Cadastrar, Read, Update, Delete
         * Adicionar Item, Remover Item, Alterar Quantidade
         */
        public void Cadastrar(Frete item)
        {
            List<Frete> Lista;
            if (_cookie.Existe(Key))
            {
                Lista = Consultar();
                var ItemLocalizado = Lista.SingleOrDefault(a => a.CEP == item.CEP);

                if (ItemLocalizado == null)
                {
                    Lista.Add(item);
                }
                else
                {
                    ItemLocalizado.CodCarrinho = item.CodCarrinho;
                    ItemLocalizado.ListaValores = item.ListaValores;
                }
            }
            else
            {
                Lista = new List<Frete>();
                Lista.Add(item);
            }

            Salvar(Lista);
        }
        public void Atualizar(Frete item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.CEP == item.CEP);

            if (ItemLocalizado != null)
            {
                ItemLocalizado.CodCarrinho = item.CodCarrinho;
                ItemLocalizado.ListaValores = item.ListaValores;
                Salvar(Lista);
            }
        }
        public void Remover(Frete item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.CEP == item.CEP);

            if (ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado);
                Salvar(Lista);
            }
        }
        public List<Frete> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<Frete>>(valor);
            }
            else
            {
                return new List<Frete>();
            }
        }
        public void Salvar(List<Frete> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        public bool Existe(string Key)
        {
            if (_cookie.Existe(Key))
            {
                return false;
            }

            return true;
        }
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
    }
}
