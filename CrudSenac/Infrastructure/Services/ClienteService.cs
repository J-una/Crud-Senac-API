using CrudSenac.Data;
using CrudSenac.Domain.Dto;
using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudSenac.Infrastructure.Services
{
    public class ClienteService : IClienteInterface
    {

        private readonly AppDbContext _context;

        public ClienteService (AppDbContext context)
        {
            _context = context;
        }

        // POST
        public async Task<Response<Cliente>> CriarCliente(Cliente novoCliente)
        {
            try
            {
                novoCliente.IdCliente = Guid.NewGuid();
                novoCliente.DataCriacao = DateTime.Now;
                novoCliente.Ativo = true;

                // Garante que o usuário não será recriado
                if (novoCliente.IdUsuario != Guid.Empty)
                {
                    var usuarioExistente = await _context.Usuarios.FindAsync(novoCliente.IdUsuario);
                    if (usuarioExistente == null)
                    {
                        return new Response<Cliente>
                        {
                            Dados = null,
                            Mensagem = "Usuário informado não existe.",
                            Status = false
                        };
                    }

                    // Impede que o EF tente inserir um novo usuário
                    novoCliente.Usuario = null;
                }

                _context.Clientes.Add(novoCliente);
                await _context.SaveChangesAsync();

                return new Response<Cliente>
                {
                    Dados = novoCliente,
                    Mensagem = "Cliente criado com sucesso.",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new Response<Cliente>
                {
                    Dados = null,
                    Mensagem = $"Erro ao criar cliente: {ex.Message}",
                    Status = false
                };
            }
        }

        // GET ALL
        public async Task<Response<List<ClienteDto>>> ListarClientes()
        {
            var clientes = await _context.Clientes
                .Include(p => p.Usuario)
                .Where(p => p.Ativo)
                .Select(p => new ClienteDto
                {
                    IdCliente = p.IdCliente,
                    Nome = p.Nome,
                    Cpf = p.Cpf,
                    Email = p.Email,
                    Senha = p.Senha,
                    DataCriacao = p.DataCriacao,
                    DataAlteracao = p.DataAlteracao,
                    Ativo = p.Ativo,
                    Usuario = new UsuarioResumoDto
                    {
                        IdUsuario = p.Usuario.IdUsuario,
                        Nome = p.Usuario.Nome
                    }
                })
                .ToListAsync();

            return new Response<List<ClienteDto>>
            {
                Dados = clientes,
                Mensagem = "Lista de clientes ativos.",
                Status = true
            };
        }

        // GET BY ID
        public async Task<Response<ClienteDto>> BuscarClientePorId(Guid idCliente)
        {
            var cliente = await _context.Clientes
               .Include(p => p.Usuario)
               .Where(p => p.IdCliente == idCliente)
               .Select(p => new ClienteDto
               {
                   IdCliente = p.IdCliente,
                   Nome = p.Nome,
                   Cpf = p.Cpf,
                   Email = p.Email,
                   Senha = p.Senha,
                   DataCriacao = p.DataCriacao,
                   DataAlteracao = p.DataAlteracao,
                   Ativo = p.Ativo,
                   Usuario = new UsuarioResumoDto
                   {
                       IdUsuario = p.Usuario.IdUsuario,
                       Nome = p.Usuario.Nome
                   }
               })
               .FirstOrDefaultAsync();

            if (cliente == null)
            {
                return new Response<ClienteDto>
                {
                    Dados = null,
                    Mensagem = "Cliente não encontrado.",
                    Status = false
                };
            }

            return new Response<ClienteDto>
            {
                Dados = cliente,
                Mensagem = "Cliente encontrado com sucesso.",
                Status = true
            };
        }

        // PUT
        public async Task<Response<Cliente>> AtualizarCliente(Guid idCliente, Cliente clienteAtualizado)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);

            if (cliente == null)
            {
                return new Response<Cliente>
                {
                    Dados = null,
                    Mensagem = "Cliente não encontrado.",
                    Status = false
                };
            }

            cliente.Nome = clienteAtualizado.Nome;
            cliente.Cpf = clienteAtualizado.Cpf;
            cliente.Email = clienteAtualizado.Email;
            cliente.Senha = clienteAtualizado.Senha;
            cliente.DataAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            return new Response<Cliente>
            {
                Dados = cliente,
                Mensagem = "Cliente atualizado com sucesso.",
                Status = true
            };
        }

        // DELETE (Inativar)
        public async Task<Response<bool>> InativarCliente(Guid idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);

            if (cliente == null)
            {
                return new Response<bool>
                {
                    Dados = false,
                    Mensagem = "Cliente não encontrado.",
                    Status = false
                };
            }

            cliente.Ativo = false;
            cliente.DataAlteracao = DateTime.Now;
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                Dados = true,
                Mensagem = "Cliente inativado com sucesso.",
                Status = true
            };
        }

        // PUT (Reativar)
        public async Task<Response<bool>> AtivarCliente(Guid idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);

            if (cliente == null)
            {
                return new Response<bool>
                {
                    Dados = false,
                    Mensagem = "Cliente não encontrado.",
                    Status = false
                };
            }

            cliente.Ativo = true;
            cliente.DataAlteracao = DateTime.Now;
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                Dados = true,
                Mensagem = "Cliente ativado com sucesso.",
                Status = true
            };
        }
    }
}
