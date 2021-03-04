using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Seguranca;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private IColaboradorRepository _repositoryColaborador;
        private LoginColaborador _loginColaborador;

        private IClienteRepository _clienteRepository;
        private IProdutoRepository _produtoRepository;
        private INewsletterRepository _newsletterRepository;
        private IPedidoRepository _pedidoRepository;
        private GerenciarEmail _gerenciarEmail;

        public HomeController(
            IClienteRepository clienteRepository,
            IProdutoRepository produtoRepository,
            INewsletterRepository newsletterRepository,
            IPedidoRepository pedidoRepository,
            IColaboradorRepository repositoryColaborador,
            LoginColaborador loginColaborador,
            GerenciarEmail gerenciarEmail
        )
        {
            _repositoryColaborador = repositoryColaborador;
            _loginColaborador = loginColaborador;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _newsletterRepository = newsletterRepository;
            _pedidoRepository = pedidoRepository;
            _gerenciarEmail = gerenciarEmail;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Models.Colaborador colaborador)
        {
            Models.Colaborador colaboradorDB = _repositoryColaborador.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDB != null)
            {
                _loginColaborador.Login(colaboradorDB);

                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }

        [ColaboradorAutorizacao]
        [ValidateHttpReferer]
        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction("Login", "Home");
        }




        [HttpGet]
        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recuperar([FromForm]Models.Colaborador colaborador)
        {
            
            var colaboradorDoBancoDados = _repositoryColaborador.ObterColaboradorPorEmail(colaborador.Email);

            if (colaboradorDoBancoDados != null && colaboradorDoBancoDados.Count > 0)
            {
                string idCrip = Base64Cipher.Base64Encode(colaboradorDoBancoDados.First().Id.ToString());
                _gerenciarEmail.EnviarLinkResetarSenha(colaboradorDoBancoDados.First(), idCrip);

                TempData["MSG_S"] = Mensagem.MSG_S004;

                ModelState.Clear();
            }
            else
            {
                TempData["MSG_E"] = Mensagem.MSG_E014;
            }
            

            return View();
        }

        [HttpGet]
        public IActionResult CriarSenha(string id)
        {
            try
            {
                var idColaboradorDecrip = Base64Cipher.Base64Decode(id);
                int idColaborador;
                if (!int.TryParse(idColaboradorDecrip, out idColaborador))
                {
                    TempData["MSG_E"] = Mensagem.MSG_E015;
                }
            }
            catch (System.FormatException)
            {
                TempData["MSG_E"] = Mensagem.MSG_E015;
            }

            return View();
        }

        [HttpPost]
        public IActionResult CriarSenha([FromForm]Models.Colaborador colaborador, string id)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Email");

            if (ModelState.IsValid)
            {
                int idColaborador;
                try
                {
                    var idColaboradorDecrip = Base64Cipher.Base64Decode(id);

                    if (!int.TryParse(idColaboradorDecrip, out idColaborador))
                    {
                        TempData["MSG_E"] = Mensagem.MSG_E015;
                        return View();
                    }



                }
                catch (System.FormatException)
                {
                    TempData["MSG_E"] = Mensagem.MSG_E015;
                    return View();
                }


                var colaboradorDB = _repositoryColaborador.ObterColaborador(idColaborador);
                if (colaboradorDB != null)
                {
                    colaboradorDB.Senha = colaborador.Senha;

                    _repositoryColaborador.AtualizarSenha(colaboradorDB);
                    TempData["MSG_S"] = Mensagem.MSG_S005;
                }

            }

            return View();
        }



        [ColaboradorAutorizacao]
        public IActionResult Painel()
        {
            ViewBag.Clientes = _clienteRepository.QuantidadeTotalClientes();
            ViewBag.Newsletter = _newsletterRepository.QuantidadeTotalNewsletters();
            ViewBag.Produto = _produtoRepository.QuantidadeTotalProdutos();
            ViewBag.NumeroPedidos = _pedidoRepository.QuantidadeTotalPedidos();
            ViewBag.ValorTotalPedidos = _pedidoRepository.ValorTotalPedidos();

            ViewBag.QuantidadeBoletoBancario = _pedidoRepository.QuantidadeTotalBoletoBancario();
            ViewBag.QuantidadeCartaoCredito = _pedidoRepository.QuantidadeTotalCartaoCredito();

            return View();
        }

        public IActionResult GerarCSVNewsletter()
        {
            var news = _newsletterRepository.ObterTodasNewsletter();

            StringBuilder sb = new StringBuilder();

            foreach (var email in news)
            {
                sb.AppendLine(email.Email);
            }

            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
            return File(buffer, "text/csv", $"newsletter.csv");
        }
    }
}