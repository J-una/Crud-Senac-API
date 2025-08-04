using CrudSenac.Data;
using CrudSenac.Domain.Dto;
using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudSenac.Infrastructure.Services
{
    public class ProdutoService : IProdutoInterface
    {
        private readonly AppDbContext _context;

        public ProdutoService (AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Produto>> CriarProduto(Produto novoProduto)
        {
            try
            {
                novoProduto.IdProduto = Guid.NewGuid();
                novoProduto.DataCriacao = DateTime.Now;
                novoProduto.Ativo = true;

                if (novoProduto.IdUsuario != Guid.Empty)
                {
                    var usuarioExistente = await _context.Usuarios.FindAsync(novoProduto.IdUsuario);
                    if (usuarioExistente == null)
                    {
                        return new Response<Produto>
                        {
                            Dados = null,
                            Mensagem = "Usuário informado não existe.",
                            Status = false
                        };
                    }

                    novoProduto.Usuario = null; // evita inserção duplicada
                }

                _context.Produtos.Add(novoProduto);
                await _context.SaveChangesAsync();

                return new Response<Produto>
                {
                    Dados = novoProduto,
                    Mensagem = "Produto criado com sucesso.",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new Response<Produto>
                {
                    Dados = null,
                    Mensagem = $"Erro ao criar produto: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<Response<ProdutoDto>> BuscarProdutoPorId(Guid idProduto)
        {
            var produto = await _context.Produtos
                .Include(p => p.Usuario)
                .Where(p => p.IdProduto == idProduto)
                .Select(p => new ProdutoDto
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    Marca = p.Marca,
                    Tipo = p.Tipo,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
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

            if (produto == null)
            {
                return new Response<ProdutoDto>
                {
                    Dados = null,
                    Mensagem = "Produto não encontrado.",
                    Status = false
                };
            }

            return new Response<ProdutoDto>
            {
                Dados = produto,
                Mensagem = "Produto encontrado com sucesso.",
                Status = true
            };
        }

        public async Task<Response<List<ProdutoDto>>> ListarProdutos()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Usuario)
                .Where(p => p.Ativo)
                .Select(p => new ProdutoDto
                {
                    IdProduto = p.IdProduto,
                    Nome = p.Nome,
                    Marca = p.Marca,
                    Tipo = p.Tipo,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
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

            return new Response<List<ProdutoDto>>
            {
                Dados = produtos,
                Mensagem = "Lista de produtos ativos retornada com sucesso.",
                Status = true
            };
        }

        public async Task<Response<Produto>> AtualizarUsuario(Guid idProduto, Produto produtoAtualizado)
        {
            var produto = await _context.Produtos.FindAsync(idProduto);

            if (produto == null)
            {
                return new Response<Produto>
                {
                    Dados = null,
                    Mensagem = "Produto não encontrado.",
                    Status = false
                };
            }

            produto.Nome = produtoAtualizado.Nome;
            produto.Marca = produtoAtualizado.Marca;
            produto.Tipo = produtoAtualizado.Tipo;
            produto.Preco = produtoAtualizado.Preco;
            produto.Quantidade = produtoAtualizado.Quantidade;
            produto.DataAlteracao = DateTime.Now;

            await _context.SaveChangesAsync();

            return new Response<Produto>
            {
                Dados = produto,
                Mensagem = "Produto atualizado com sucesso.",
                Status = true
            };
        }

        public async Task<Response<bool>> InativarProduto(Guid idProduto)
        {
            var produto = await _context.Produtos.FindAsync(idProduto);

            if (produto == null)
            {
                return new Response<bool>
                {
                    Dados = false,
                    Mensagem = "Produto não encontrado.",
                    Status = false
                };
            }

            produto.Ativo = false;
            produto.DataAlteracao = DateTime.Now;
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                Dados = true,
                Mensagem = "Produto inativado com sucesso.",
                Status = true
            };
        }

        public async Task<Response<bool>> AtivarProduto(Guid idProduto)
        {
            var produto = await _context.Produtos.FindAsync(idProduto);

            if (produto == null)
            {
                return new Response<bool>
                {
                    Dados = false,
                    Mensagem = "Produto não encontrado.",
                    Status = false
                };
            }

            produto.Ativo = true;
            produto.DataAlteracao = DateTime.Now;
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                Dados = true,
                Mensagem = "Produto ativado com sucesso.",
                Status = true
            };
        }

    }
}
