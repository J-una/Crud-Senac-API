using CrudSenac.Data;
using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudSenac.Infrastructure.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        public readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Usuario>> CriarUsuario(Usuario novoUsuario)
        {
            try
            {
                novoUsuario.IdUsuario = Guid.NewGuid();
                novoUsuario.DataCriacao = DateTime.Now;
                novoUsuario.Ativo = true;

                _context.Usuarios.Add(novoUsuario);
                await _context.SaveChangesAsync();

                return new Response<Usuario>
                {
                    Dados = novoUsuario,
                    Mensagem = "Usuário criado com sucesso.",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new Response<Usuario>
                {
                    Dados = null,
                    Mensagem = $"Erro ao criar usuário: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<Response<List<Usuario>>> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Ativo)
                .ToListAsync();

            return new Response<List<Usuario>>
            {
                Dados = usuarios,
                Mensagem = "Lista de usuários ativos retornada com sucesso.",
                Status = true
            };
        }

        public async Task<Response<Usuario>> BuscarUsuarioPorId(Guid idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null)
            {
                return new Response<Usuario>
                {
                    Dados = null,
                    Mensagem = "Usuário não encontrado.",
                    Status = false
                };
            }

            return new Response<Usuario>
            {
                Dados = usuario,
                Mensagem = "Usuário encontrado com sucesso.",
                Status = true
            };
        }

        public async Task<Response<Usuario>> AtualizarUsuario(Guid idUsuario, Usuario usuarioAtualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null)
            {
                return new Response<Usuario>
                {
                    Dados = null,
                    Mensagem = "Usuário não encontrado.",
                    Status = false
                };
            }

            usuario.Nome = usuarioAtualizado.Nome;
            usuario.Cpf = usuarioAtualizado.Cpf;
            usuario.Senha = usuarioAtualizado.Senha;
            usuario.DataAlteracao = DateTime.Now;
            usuario.Perfil = usuarioAtualizado.Perfil;

            await _context.SaveChangesAsync();

            return new Response<Usuario>
            {
                Dados = usuario,
                Mensagem = "Usuário atualizado com sucesso.",
                Status = true
            };
        }

        public async Task<Response<bool>> InativarUsuario(Guid idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null)
            {
                return new Response<bool>
                {
                    Dados = false,
                    Mensagem = "Usuário não encontrado.",
                    Status = false
                };
            }

            usuario.Ativo = false;
            usuario.DataAlteracao = DateTime.Now;
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                Dados = true,
                Mensagem = "Usuário inativado com sucesso.",
                Status = true
            };
        }

        public async Task<Response<bool>> AtivarUsuario(Guid idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null)
            {
                return new Response<bool>
                {
                    Dados = false,
                    Mensagem = "Usuário não encontrado.",
                    Status = false
                };
            }

            usuario.Ativo = true;
            usuario.DataAlteracao = DateTime.Now;
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                Dados = true,
                Mensagem = "Usuário ativado com sucesso.",
                Status = true
            };
        }
    }
}
