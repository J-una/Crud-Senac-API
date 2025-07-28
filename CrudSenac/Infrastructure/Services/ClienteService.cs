using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;

namespace CrudSenac.Infrastructure.Services
{
    public class ClienteService : IClienteInterface
    {
        public Task<Response<Cliente>> AtualizarCliente(Guid idCliente, Cliente clienteAtualizado)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Cliente>> BuscarClientePorId(Guid idCliente)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Cliente>> CriarCliente(Cliente novoCliente)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> InativarCliente(Guid idCliente)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Usuario>>> ListarClientes()
        {
            throw new NotImplementedException();
        }
    }
}
