using CrudSenac.Data;
using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;

namespace CrudSenac.Infrastructure.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        public readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public Task<Response<Usuario>> AtualizarUsuario(Guid idUsuario, Usuario usuarioAtualizado)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Usuario>> BuscarUsuarioPorId(Guid idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Usuario>> CriarUsuario(Usuario novoUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> InativarUsuario(Guid idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Usuario>>> ListarUsuarios()
        {
            throw new NotImplementedException();
        }
    }
}
