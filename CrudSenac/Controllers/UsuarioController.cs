using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudSenac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioService;

        public UsuarioController(IUsuarioInterface usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // POST: api/usuario/adicionar-usuario
        [HttpPost, Route("adicionar-usuario")]
        public async Task<IActionResult> CriarUsuario([FromBody] Usuario usuario)
        {
            var response = await _usuarioService.CriarUsuario(usuario);
            return CreatedAtAction(nameof(BuscarUsuarioPorId), new { id = response.Dados?.IdUsuario }, response);
        }

        // GET: api/usuario/buscar-usuarios
        [HttpGet, Route("buscar-usuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var response = await _usuarioService.ListarUsuarios();
            return Ok(response);
        }

        // GET: api/usuario/buscar-usuario/{id}
        [HttpGet, Route("buscar-usuario/{id}")]
        public async Task<IActionResult> BuscarUsuarioPorId(Guid id)
        {
            var response = await _usuarioService.BuscarUsuarioPorId(id);
            if (response.Dados == null)
                return NotFound(response);
            return Ok(response);
        }

        // PUT: api/usuario/editar-usuario?id={id}
        [HttpPut, Route("editar-usuario")]
        public async Task<IActionResult> AtualizarUsuario(Guid id, [FromBody] Usuario usuarioAtualizado)
        {
            var response = await _usuarioService.AtualizarUsuario(id, usuarioAtualizado);
            if (response.Dados == null)
                return NotFound(response);
            return Ok(response);
        }

        // DELETE: api/usuario/inativar-usuario/{id}
        [HttpDelete, Route("inativar-usuario/{id}")]
        public async Task<IActionResult> InativarUsuario(Guid id)
        {
            var response = await _usuarioService.InativarUsuario(id);
            if (!response.Dados)
                return NotFound(response);
            return Ok(response);
        }

        // PUT: api/usuario/ativar-usuario/{id}
        [HttpPut, Route("ativar-usuario/{id}")]
        public async Task<IActionResult> AtivarUsuario(Guid id)
        {
            var response = await _usuarioService.AtivarUsuario(id);
            if (!response.Dados)
                return NotFound(response);
            return Ok(response);
        }

    }
}
