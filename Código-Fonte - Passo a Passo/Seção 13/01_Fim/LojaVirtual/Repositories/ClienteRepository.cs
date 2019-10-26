using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;

namespace LojaVirtual.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private LojaVirtualContext _banco;

        public ClienteRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }

        public void Atualizar(Cliente cliente)
        {
            _banco.Update(cliente);
            _banco.SaveChanges();
        }

        public void Cadastrar(Cliente cliente)
        {
            _banco.Add(cliente);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Cliente cliente = ObterCliente(Id);
            _banco.Remove(cliente);
            _banco.SaveChanges();
        }

        public Cliente Login(string Email, string Senha)
        {
            Cliente cliente = _banco.Clientes.Where(m => m.Email == Email && m.Senha == Senha).FirstOrDefault();
            return cliente;
        }

        public Cliente ObterCliente(int Id)
        {
            return _banco.Clientes.Find(Id);
        }

        public IEnumerable<Cliente> ObterTodosClientes()
        {
            return _banco.Clientes.ToList();
        }
    }
}
