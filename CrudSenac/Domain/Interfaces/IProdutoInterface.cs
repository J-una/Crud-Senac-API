using CrudSenac.Domain.Entities;

namespace CrudSenac.Domain.Interfaces
{
    public interface IProdutoInterface
    {

        //Post
        Task<Response<Produto>> CriarProduto(Produto novoProduto);

        //Get
        Task<Response<List<Produto>>> ListarProdutos();
        Task<Response<Produto>> BuscarProdutoPorId(Guid idProduto);

        //Update
        Task<Response<Produto>> AtualizarUsuario(Guid idProduto, Usuario produtoAtualizado);

        //Delete
        Task<Response<bool>> InativarProduto(Guid idProduto);
    }
}
