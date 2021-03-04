using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories.Contracts
{
    public interface ICategoriaRepository
    {
        //CRUD
        void Cadastrar(Categoria categoria);
        void Atualizar(Categoria categoria);
        void Excluir(int id);
        Categoria ObterCategoria(int id);
        Categoria ObterCategoria(string slug);
        List<Categoria> ObterCategoriasPorCategoriaPai(int id);
        Categoria ObterCategoriaPorNome(string nome);
        IEnumerable<Categoria> ObterCategoriasRecursivas(Categoria categoriaPai);
        IEnumerable<Categoria> ObterTodasCategorias();        
        IPagedList<Categoria> ObterTodasCategorias(int? pagina);
    }
}
