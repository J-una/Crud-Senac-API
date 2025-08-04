using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudSenac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteInterface _clienteService;

        public ClienteController(IClienteInterface cliente)
        {
            _clienteService = cliente;
        }

        // POST: api/cliente
        [HttpPost, Route("adicionar-cliente")]
        public async Task<IActionResult> CriarCliente([FromBody] Cliente cliente)
        {
            var response = await _clienteService.CriarCliente(cliente);
            return CreatedAtAction(nameof(BuscarClientePorId), new { id = response.Dados?.IdCliente }, response);
        }

        // GET: api/cliente
        [HttpGet, Route("buscar-clientes")]
        public async Task<IActionResult> ListarClientes()
        {
            var response = await _clienteService.ListarClientes();
            return Ok(response);
        }

        // GET: api/cliente/{id}
        [HttpGet, Route("buscar-cliente/{id}")]
        public async Task<IActionResult> BuscarClientePorId(Guid id)
        {
            var response = await _clienteService.BuscarClientePorId(id);
            if (response.Dados == null)
                return NotFound(response);
            return Ok(response);
        }

        // PUT: api/cliente/{id}
        [HttpPut, Route("editar-cliente")]
        public async Task<IActionResult> AtualizarCliente(Guid id, [FromBody] Cliente clienteAtualizado)
        {
            var response = await _clienteService.AtualizarCliente(id, clienteAtualizado);
            if (response.Dados == null)
                return NotFound(response);
            return Ok(response);
        }

        // DELETE: api/cliente/inativar/{id}
        [HttpDelete, Route("inativar-cliente/{id}")]
        public async Task<IActionResult> InativarCliente(Guid id)
        {
            var response = await _clienteService.InativarCliente(id);
            if (!response.Dados)
                return NotFound(response);
            return Ok(response);
        }

        // PUT: api/cliente/ativar/{id}
        [HttpPut, Route("ativar-cliente/{id}")]
        public async Task<IActionResult> AtivarCliente(Guid id)
        {
            var response = await _clienteService.AtivarCliente(id);
            if (!response.Dados)
                return NotFound(response);
            return Ok(response);
        }

    }
}
