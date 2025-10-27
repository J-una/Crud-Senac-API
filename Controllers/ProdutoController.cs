using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using CrudSenac.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudSenac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {

        private readonly IProdutoInterface _produtoService;

        public ProdutoController (IProdutoInterface produto)
        {
            _produtoService = produto;
        }


        // POST: api/produto/adicionar-produto
        [HttpPost, Route("adicionar-produto")]
        public async Task<IActionResult> CriarProduto([FromBody] Produto produto)
        {
            var response = await _produtoService.CriarProduto(produto);
            return CreatedAtAction(nameof(BuscarProdutoPorId), new { id = response.Dados?.IdProduto }, response);
        }

        // GET: api/produto/buscar-produtos
        [HttpGet, Route("buscar-produtos")]
        public async Task<IActionResult> ListarProdutos()
        {
            var response = await _produtoService.ListarProdutos();
            return Ok(response);
        }

        // GET: api/produto/buscar-produto/{id}
        [HttpGet, Route("buscar-produto/{id}")]
        public async Task<IActionResult> BuscarProdutoPorId(Guid id)
        {
            var response = await _produtoService.BuscarProdutoPorId(id);
            if (response.Dados == null)
                return NotFound(response);
            return Ok(response);
        }

        // PUT: api/produto/editar-produto?id={id}
        [HttpPut, Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id, [FromBody] Produto produtoAtualizado)
        {
            var response = await _produtoService.AtualizarUsuario(id, produtoAtualizado);
            if (response.Dados == null)
                return NotFound(response);
            return Ok(response);
        }

        // DELETE: api/produto/inativar-produto/{id}
        [HttpDelete, Route("inativar-produto/{id}")]
        public async Task<IActionResult> InativarProduto(Guid id)
        {
            var response = await _produtoService.InativarProduto(id);
            if (!response.Dados)
                return NotFound(response);
            return Ok(response);
        }

        // PUT: api/produto/ativar-produto/{id}
        [HttpPut, Route("ativar-produto/{id}")]
        public async Task<IActionResult> AtivarProduto(Guid id)
        {
            var response = await _produtoService.AtivarProduto(id);
            if (!response.Dados)
                return NotFound(response);
            return Ok(response);
        }
    }

}

