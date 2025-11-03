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
                novoUsuario.Email = novoUsuario.Email.ToLower();
                novoUsuario.Ativo = true;

                if (string.IsNullOrWhiteSpace(novoUsuario.Senha))
                    throw new ArgumentException("Senha é obrigatória.");
                novoUsuario.Senha = BCrypt.Net.BCrypt.HashPassword(novoUsuario.Senha, workFactor: 10);

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
            usuario.DataAlteracao = DateTime.Now;
            usuario.Email = usuarioAtualizado.Email.ToLower();
            usuario.Perfil = usuarioAtualizado.Perfil;

            if (!string.IsNullOrWhiteSpace(usuarioAtualizado.Senha))
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizado.Senha, workFactor: 10);
            }

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

        public async Task<bool> EmailExiste(string email, Guid? id = null)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && (id == null || u.IdUsuario != id));
        }

        public async Task<bool> CpfExiste(string cpf, Guid? id = null)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Cpf == cpf && (id == null || u.IdUsuario != id));
        }
    }
}
