using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Models;
using LojaVirtual.Repositories;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Cliente.Controllers
{
    [Area("Cliente")]
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



        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]EnderecoEntrega enderecoentrega, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                enderecoentrega.ClienteId = _loginCliente.GetCliente().Id;

                _enderecoEntregaRepository.Cadastrar(enderecoentrega);

                if (returnUrl == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Models.Cliente cliente = _loginCliente.GetCliente();
            EnderecoEntrega endereco = _enderecoEntregaRepository.ObterEnderecoEntrega( id );

            if(endereco.ClienteId != cliente.Id)
            {
                return new ContentResult() { Content = "Acesso negado." };
            }

            return View(endereco);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm]EnderecoEntrega enderecoentrega)
        {
            if (ModelState.IsValid)
            {
                enderecoentrega.ClienteId = _loginCliente.GetCliente().Id;

                _enderecoEntregaRepository.Atualizar(enderecoentrega);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            Models.Cliente cliente = _loginCliente.GetCliente();
            EnderecoEntrega enderecoEntrega = _enderecoEntregaRepository.ObterEnderecoEntrega(id);
            if (cliente.Id == enderecoEntrega.ClienteId)
            {
                _enderecoEntregaRepository.Excluir(id);
                TempData["MSG_S"] = Mensagem.MSG_S002;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return new ContentResult() { Content = "Acesso negado." };
            }
        }
        //CRUD - Cadastro-OK, Listagem-OK, Atualização-OK, Remover-OK
    }
}