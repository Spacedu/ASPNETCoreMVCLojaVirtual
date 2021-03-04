using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [ClienteAutorizacao]
    public class PedidoController : Controller
    {
        private LoginCliente _loginCliente;
        private IPedidoRepository _pedidoRepository;

        public PedidoController(LoginCliente loginCliente, IPedidoRepository pedidoRepository)
        {
            _loginCliente = loginCliente;
            _pedidoRepository = pedidoRepository;
        }

        public IActionResult Index(int? pagina)
        {
            Models.Cliente cliente = _loginCliente.GetCliente();
            var pedidos = _pedidoRepository.ObterTodosPedidoCliente(pagina, cliente.Id);

            return View(pedidos);
        }

        public IActionResult Visualizar(int id)
        {
            Models.Cliente cliente = _loginCliente.GetCliente();
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (pedido.ClienteId != cliente.Id)
            {
                return new StatusCodeResult(403);
            }

            return View(pedido);
        }
    }
}