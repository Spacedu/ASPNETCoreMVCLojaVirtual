using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Json.Resolver;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LojaVirtual.Controllers
{
    [ClienteAutorizacao]
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private LoginCliente _loginCliente;
        public PedidoController(IPedidoRepository pedidoRepository, LoginCliente loginCliente)
        {
            _pedidoRepository = pedidoRepository;
            _loginCliente = loginCliente;
        }
        public IActionResult Index(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if(pedido == null)
            {
                return new StatusCodeResult(404);
            }

            if (pedido.ClienteId != _loginCliente.GetCliente().Id)
            {
                return new StatusCodeResult(403);
            }

            ViewBag.Produtos = JsonConvert.DeserializeObject<List<ProdutoItem>>( 
                pedido.DadosProdutos,
                new JsonSerializerSettings() { ContractResolver = new ProdutoItemResolver<List<ProdutoItem>>() }
            );

            var transacao = JsonConvert.DeserializeObject<TransacaoPagarMe>(pedido.DadosTransaction);
            
            ViewBag.Transacao = transacao;

            return View(pedido);
        }
    }
}