using CrudSenac.Data;
using CrudSenac.Domain.Dto;
using CrudSenac.Domain.Entities;
using CrudSenac.Domain.Interfaces;
using CrudSenac.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSenac.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AutenticacaoController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogintDto request)
        {
            if (string.IsNullOrEmpty(request.Cpf) || string.IsNullOrEmpty(request.Senha))
                return BadRequest("CPF e senha são obrigatórios");

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Cpf == request.Cpf && u.Ativo);

            if (usuario == null)
                return Unauthorized("CPF ou senha inválidos");

            bool senhaValida;

            // Estratégia de migração (opcional):
            // Se você já tem senhas em texto no banco, pode detectar se está em bcrypt
            // e, caso não esteja, comparar plain-text e re-hashear na primeira autenticação.
            if (!string.IsNullOrEmpty(usuario.Senha) && usuario.Senha.StartsWith("$2")) // bcrypt hash começa com $2a/$2b/$2y
            {
                senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha);
            }
            else
            {
                // Atenção: só usar isso se você estiver migrando de senhas em texto puro.
                // Se não existir essa situação no seu DB, remova esse bloco e retorne Unauthorized direto.
                senhaValida = usuario.Senha == request.Senha;
                if (senhaValida)
                {
                    // Re-hash e salvar
                    usuario.Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha, workFactor: 10);
                    usuario.DataAlteracao = DateTime.UtcNow;
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();
                }
            }

            if (!senhaValida)
                return Unauthorized("CPF ou senha inválidos");

            var response = new LoginResponseDto
            {
                Token = Guid.NewGuid().ToString(), // aqui idealmente você gera um JWT
                Nome = usuario.Nome,
                Perfil = usuario.Perfil,
                IdUsuario = usuario.IdUsuario
            };

            return Ok(response);
        }



        [HttpPost("solicitar-recuperacao")]
        public async Task<IActionResult> SolicitarRecuperacao([FromBody] SolicitarRecuperacaoDto request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            var token = Guid.NewGuid().ToString();

            var recuperacao = new RecuperacaoSenha
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.IdUsuario,
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddHours(1),
                Utilizado = false
            };

            _context.RecuperacoesSenha.Add(recuperacao);
            await _context.SaveChangesAsync();

            // Enviar e-mail real
            var link = $"http://localhost:4200/redefinir-senha?token={token}";
            var assunto = "Recuperação de Senha - CRUD Senac";
            var corpo = $"Olá {usuario.Nome},<br/><br/>" +
                        $"Clique no link abaixo para redefinir sua senha:<br/>" +
                        $"<a href='{link}'>{link}</a><br/><br/>" +
                        $"O link expira em 1 hora.";

            await _emailService.EnviarEmailAsync(usuario.Email, assunto, corpo);

            return Ok($"Um link de recuperação foi enviado para {usuario.Email}.");
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaDto request)
        {
            var recuperacao = await _context.RecuperacoesSenha
                .FirstOrDefaultAsync(r => r.Token == request.Token && !r.Utilizado);

            if (recuperacao == null || recuperacao.ExpiraEm < DateTime.UtcNow)
                return BadRequest("Token inválido ou expirado.");

            var usuario = await _context.Usuarios.FindAsync(recuperacao.UsuarioId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            if (string.IsNullOrWhiteSpace(request.NovaSenha))
                return BadRequest("Nova senha inválida.");

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(request.NovaSenha, workFactor: 10);
            usuario.DataAlteracao = DateTime.UtcNow;

            recuperacao.Utilizado = true;

            _context.Usuarios.Update(usuario);
            _context.RecuperacoesSenha.Update(recuperacao);
            await _context.SaveChangesAsync();

            return Ok("Senha redefinida com sucesso!");
        }
    }
  }
