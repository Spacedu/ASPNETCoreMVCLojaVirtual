using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Repositories.Contracts
{
    public interface IEnderecoEntregaRepository
    {
        void Cadastrar(EnderecoEntrega endereco);
        void Atualizar(EnderecoEntrega endereco);
        void Excluir(int Id);
        EnderecoEntrega ObterEnderecoEntrega(int Id);
        IList<EnderecoEntrega> ObterTodosEnderecoEntregaCliente(int ClienteId);
    }
}
