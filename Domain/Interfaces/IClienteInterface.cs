using CrudSenac.Domain.Entities;

namespace CrudSenac.Domain.Interfaces
{
    public interface IClienteInterface
    {
        //Post
        Task<Response<Cliente>> CriarCliente(Cliente novoCliente);

        //Get
        Task<Response<List<Cliente>>> ListarClientes();
        Task<Response<Cliente>> BuscarClientePorId(Guid idCliente);

        //Update
        Task<Response<Cliente>> AtualizarCliente(Guid idCliente, Cliente clienteAtualizado);

        //Delete
        Task<Response<bool>> InativarCliente(Guid idCliente);

        Task<Response<bool>> AtivarCliente(Guid idCliente);

    }
}
