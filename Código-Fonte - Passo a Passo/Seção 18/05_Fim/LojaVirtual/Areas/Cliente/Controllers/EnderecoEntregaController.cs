using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Repositories;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Cliente.Controllers
{
    [ClienteAutorizacao]
    public class EnderecoEntregaController : Controller
    {
        private LoginCliente _loginCliente;
        private IEnderecoEntregaRepository _enderecoEntregaRepository;

        public EnderecoEntregaController(LoginCliente loginCliente, IEnderecoEntregaRepository enderecoEntregaRepository)
        {
            _loginCliente = loginCliente;
            _enderecoEntregaRepository = enderecoEntregaRepository;
        }
        public IActionResult Index()
        {
            var cliente = _loginCliente.GetCliente();
            ViewBag.Cliente = cliente;
            ViewBag.Enderecos = _enderecoEntregaRepository.ObterTodosEnderecoEntregaCliente(cliente.Id);

            return View();
        }

        //CRUD - Cadastro, Listagem-OK, Atualização, Remover
    }
}