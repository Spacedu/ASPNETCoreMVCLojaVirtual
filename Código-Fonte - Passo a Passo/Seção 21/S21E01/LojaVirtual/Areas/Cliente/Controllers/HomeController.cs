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
    public class HomeController : Controller
    {
        private IEnderecoEntregaRepository _repositoryEnderecoEntrega;
        private IClienteRepository _repositoryCliente;
        private LoginCliente _loginCliente;

        public HomeController(IEnderecoEntregaRepository repositoryEnderecoEntrega, IClienteRepository repositoryCliente, LoginCliente loginCliente)
        {
            _repositoryEnderecoEntrega = repositoryEnderecoEntrega;
            _repositoryCliente = repositoryCliente;
            _loginCliente = loginCliente;            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Models.Cliente cliente, string returnUrl = null)
        {
            Models.Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                _loginCliente.Login(clienteDB);

                if(returnUrl == null)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }                
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Sair()
        {
            _loginCliente.Logout();

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}