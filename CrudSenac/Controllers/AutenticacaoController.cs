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
                .FirstOrDefaultAsync(u => u.Cpf == request.Cpf && u.Senha == request.Senha && u.Ativo);

            if (usuario == null)
                return Unauthorized("CPF ou senha inválidos");

            // ⚠️ Aqui, por simplicidade, estamos sem JWT
            // Se quiser JWT, adicionamos depois

            var response = new LoginResponseDto
            {
                Token = Guid.NewGuid().ToString(), // apenas simulação
                Nome = usuario.Nome,
                Perfil = usuario.Perfil
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

            usuario.Senha = request.NovaSenha; // ⚠️ ideal: salvar senha com hash
            usuario.DataAlteracao = DateTime.UtcNow;

            recuperacao.Utilizado = true;

            _context.Usuarios.Update(usuario);
            _context.RecuperacoesSenha.Update(recuperacao);
            await _context.SaveChangesAsync();

            return Ok("Senha redefinida com sucesso!");
        }
    }
  }
