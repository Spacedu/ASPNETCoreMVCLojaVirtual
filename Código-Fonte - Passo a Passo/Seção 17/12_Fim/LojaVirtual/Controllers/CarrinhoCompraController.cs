using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private CarrinhoCompra _carrinhoCompra;
        private IProdutoRepository _produtoRepository;

        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository)
        {
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
        }
        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemNoCarrinho = _carrinhoCompra.Consultar();

            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemNoCarrinho)
            {
                //TODO - Implementar AutoMapper...
                Produto produto = _produtoRepository.ObterProduto(item.Id);

                ProdutoItem produtoItem = new ProdutoItem();
                produtoItem.Id = produto.Id;
                produtoItem.Nome = produto.Nome;
                produtoItem.Imagens = produto.Imagens;
                produtoItem.Valor = produto.Valor;
                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);
            }

            return View(produtoItemCompleto);
        }

        //Item ID = ID Produto
        public IActionResult AdicionarItem(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if(produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                //TODO - Caso o produto já exista, deve ser adicionar uma quantidade maior ao inves adicionar um novo objeto a lista.
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = 1 };
                _carrinhoCompra.Cadastrar(item);

                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
            var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = quantidade };
            _carrinhoCompra.Atualizar(item);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoverItem(int id)
        {
            _carrinhoCompra.Remover(new ProdutoItem() { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}