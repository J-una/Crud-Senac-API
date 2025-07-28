using CrudSenac.Domain.Entities;

namespace CrudSenac.Domain.Interfaces
{
    public interface IUsuarioInterface
    {

        //Post
        Task<Response<Usuario>> CriarUsuario(Usuario novoUsuario);

        //Get
        Task<Response<List<Usuario>>> ListarUsuarios();
        Task<Response<Usuario>> BuscarUsuarioPorId(Guid idUsuario);

        //Update
        Task<Response<Usuario>> AtualizarUsuario(Guid idUsuario, Usuario usuarioAtualizado);

        //Delete
        Task<Response<bool>> InativarUsuario(Guid idUsuario);

    }
}
