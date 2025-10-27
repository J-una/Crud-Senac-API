using CrudSenac.Domain.Dto;
using CrudSenac.Domain.Entities;

namespace CrudSenac.Domain.Interfaces
{
    public interface IProdutoInterface
    {

        //Post
        Task<Response<Produto>> CriarProduto(Produto novoProduto);

        //Get
        Task<Response<List<ProdutoDto>>> ListarProdutos();
        Task<Response<ProdutoDto>> BuscarProdutoPorId(Guid idProduto);

        //Update
        Task<Response<Produto>> AtualizarUsuario(Guid idProduto, Produto produtoAtualizado);

        //Delete
        Task<Response<bool>> InativarProduto(Guid idProduto);

        Task<Response<bool>> AtivarProduto(Guid idProduto);
    }
}
