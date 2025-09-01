using System.Net.Mail;
using System.Net;
using CrudSenac.Domain.Interfaces;

namespace CrudSenac.Infrastructure.Services
{
    public class EmailService : IEmailInterface
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarEmailAsync(string para, string assunto, string mensagem)
        {
            var emailRemetente = _configuration["EmailSettings:Email"];
            var senha = _configuration["EmailSettings:Senha"];
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

            var mail = new MailMessage();
            mail.From = new MailAddress(emailRemetente, "Senac App");
            mail.To.Add(para);
            mail.Subject = assunto;
            mail.Body = mensagem;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(emailRemetente, senha),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
