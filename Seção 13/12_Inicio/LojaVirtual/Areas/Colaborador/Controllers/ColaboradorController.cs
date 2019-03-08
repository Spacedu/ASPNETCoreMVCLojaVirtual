using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Texto;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRespository;
        private GerenciarEmail _gerenciarEmail;

        public ColaboradorController(IColaboradorRepository colaboradorRespository, GerenciarEmail gerenciarEmail)
        {
            _colaboradorRespository = colaboradorRespository;
            _gerenciarEmail = gerenciarEmail;
        }

        public IActionResult Index(int? pagina)
        {
            IPagedList<Models.Colaborador> colaboradores = _colaboradorRespository.ObterTodosColaboradores(pagina);

            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Models.Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                //TODO - Gerar Senha Aleatorio, Enviar o E-mail

                colaborador.Tipo = "C";
                _colaboradorRespository.Cadastrar(colaborador);


                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult GerarSenha(int id)
        {
            Models.Colaborador colaborador = _colaboradorRespository.ObterColaborador(id);
            colaborador.Senha = KeyGenerator.GetUniqueKey(8);
            _colaboradorRespository.Atualizar(colaborador);

            _gerenciarEmail.EnviarSenhaParaColaboradorPorEmail(colaborador);

            TempData["MSG_S"] = Mensagem.MSG_S003;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colaborador = _colaboradorRespository.ObterColaborador(id);
            return View(colaborador);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm]Models.Colaborador colaborador, int id)
        {
            if(ModelState.IsValid)
            {
                _colaboradorRespository.Atualizar(colaborador);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            _colaboradorRespository.Excluir(id);

            TempData["MSG_S"] = Mensagem.MSG_S002;

            return RedirectToAction(nameof(Index));
        }
    }
}