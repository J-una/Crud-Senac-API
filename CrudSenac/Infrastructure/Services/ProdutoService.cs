using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;

namespace CrudSenac.Infrastructure.Services
{
    public class ProdutoService : IProdutoInterface
    {
        public Task<Response<Produto>> AtualizarUsuario(Guid idProduto, Usuario produtoAtualizado)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Produto>> BuscarProdutoPorId(Guid idProduto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Produto>> CriarProduto(Produto novoProduto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> InativarProduto(Guid idProduto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Produto>>> ListarProdutos()
        {
            throw new NotImplementedException();
        }
    }
}
