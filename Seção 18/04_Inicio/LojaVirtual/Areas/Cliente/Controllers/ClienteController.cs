using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Cliente.Controllers
{
    public class ClienteController : Controller
    {
        private LoginCliente _loginCliente;
        private ClienteRepository _clienteRepository;

        public ClienteController(LoginCliente loginCliente, ClienteRepository clienteRepository)
        {
            _loginCliente = loginCliente;
            _clienteRepository = clienteRepository;
        }

        [ClienteAutorizacao]
        public IActionResult Index()
        {
            return View();
        }

        [ClienteAutorizacao]
        [HttpGet]
        public IActionResult Atualizar()
        {
            Models.Cliente cliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().Id);
            
            return View(cliente);
        }

        [ClienteAutorizacao]
        [HttpPost]
        public IActionResult Atualizar(Models.Cliente cliente)
        {
            ModelState.Remove("Senha");
            ModelState.Remove("ConfirmacaoSenha");

            if (ModelState.IsValid)
            {
                cliente.Senha = _loginCliente.GetCliente().Senha;
                _clienteRepository.Atualizar(cliente);

                _loginCliente.Login(cliente);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
                
            }
            return View();
        }
    }
}