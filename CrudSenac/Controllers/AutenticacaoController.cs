using CrudSenac.Data;
using CrudSenac.Domain.Dto;
using CrudSenac.Domain.Interfaces;
using CrudSenac.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSenac.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class AutenticacaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailInterface _emailService;

        public AutenticacaoController(AppDbContext context,
            IEmailInterface emailInterface)
        {
            _context = context;
            _emailService = emailInterface;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Cpf) || string.IsNullOrEmpty(loginDto.Senha))
                return BadRequest("CPF e senha são obrigatórios.");

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Cpf == loginDto.Cpf && u.Senha == loginDto.Senha && u.Ativo);

            if (usuario == null)
                return Unauthorized("CPF ou senha incorretos.");

            return Ok(new
            {
                usuario.IdUsuario,
                usuario.Nome,
                usuario.Cpf,
                usuario.Perfil
            });
        }

        [HttpPost("solicitar-reset-senha")]
        public async Task<IActionResult> SolicitarResetSenha([FromBody] string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
                return BadRequest("Usuário não encontrado.");

            var token = Guid.NewGuid().ToString();
            usuario.ResetToken = token;
            usuario.ResetTokenExpiracao = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            var linkReset = $"https://seusite.com/reset-senha?token={token}";
            var mensagem = $"<p>Olá {usuario.Nome},</p>" +
                           $"<p>Clique no link abaixo para redefinir sua senha:</p>" +
                           $"<a href='{linkReset}'>Redefinir Senha</a>" +
            $"<p>Este link expira em 1 hora.</p>";

            await _emailService.EnviarEmailAsync(usuario.Email, "Recuperação de Senha", mensagem);

            return Ok("E-mail de recuperação enviado.");
        }


    }

}
