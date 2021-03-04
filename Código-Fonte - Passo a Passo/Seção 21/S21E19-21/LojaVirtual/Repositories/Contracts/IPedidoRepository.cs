using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories.Contracts
{
    public interface IPedidoRepository
    {
        //CRUD
        void Cadastrar(Pedido pedido);
        void Atualizar(Pedido pedido);        
        Pedido ObterPedido(int Id);
        IPagedList<Pedido> ObterTodosPedidoCliente(int? pagina, int clienteId);

        IPagedList<Pedido> ObterTodosPedido(int? pagina, string codigoPedido, string cpf);
        List<Pedido> ObterTodosPedidosPorSituacao(string status);
        int QuantidadeTotalPedidos();
        decimal ValorTotalPedidos();
        int QuantidadeTotalCartaoCredito();
        int QuantidadeTotalBoletoBancario();
    }
}
