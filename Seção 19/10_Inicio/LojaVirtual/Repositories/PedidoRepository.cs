using LojaVirtual.Database;
using LojaVirtual.Libraries.Texto;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        IConfiguration _conf;
        LojaVirtualContext _banco;

        public PedidoRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public void Atualizar(Pedido pedido)
        {
            _banco.Update(pedido);
            _banco.SaveChanges();
        }

        public void Cadastrar(Pedido pedido)
        {
            _banco.Add(pedido);
            _banco.SaveChanges();
        }

        public Pedido ObterPedido(int Id)
        {
            return _banco.Pedidos.Include(a => a.PedidoSituacoes).Where(a => a.Id == Id).FirstOrDefault();
        }

        public IPagedList<Pedido> ObterTodosPedido(int? pagina, string codigoPedido, string cpf)
        {
            int RegistroPorPagina = _conf.GetValue<int>("RegistroPorPagina");

            int NumeroPagina = pagina ?? 1;

            var query = _banco.Pedidos.Include(a => a.PedidoSituacoes).Include(a=>a.Cliente).AsQueryable();

            if (cpf != null)
            {
                query = query.Where(a=>a.Cliente.CPF == cpf);
            }
            if (codigoPedido != null)
            {
                string transacaoId = string.Empty;
                int id = Mascara.ExtrairCodigoPedido(codigoPedido, out transacaoId);

                query = query.Where(a => a.Id == id && a.TransactionId == transacaoId);
            }


            return query.ToPagedList<Pedido>(NumeroPagina, RegistroPorPagina);
        }

        public IPagedList<Pedido> ObterTodosPedidoCliente(int? pagina, int clienteId)
        {
            int RegistroPorPagina = _conf.GetValue<int>("RegistroPorPagina");

            int NumeroPagina = pagina ?? 1;

            return _banco.Pedidos.Include(a => a.PedidoSituacoes).ToPagedList<Pedido>(NumeroPagina, RegistroPorPagina);
        }

        public List<Pedido> ObterTodosPedidosRealizados()
        {
            return _banco.Pedidos.Include(a => a.PedidoSituacoes).Include(a=>a.Cliente).Where(a => a.Situacao == PedidoSituacaoConstant.PEDIDO_REALIZADO).ToList();
        }
    }
}
