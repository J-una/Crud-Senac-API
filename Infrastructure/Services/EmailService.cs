using CrudSenac.Domain.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CrudSenac.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com"; // servidor SMTP
        private readonly int _smtpPort = 587; // porta TLS
        private readonly string _smtpUser = "juanenvioemail@gmail.com";
        private readonly string _smtpPass = ""; // Use senha de app se Gmail

        public async Task EnviarEmailAsync(string para, string assunto, string corpo)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_smtpUser, "CRUD Senac");
            mail.To.Add(para);
            mail.Subject = assunto;
            mail.Body = corpo;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(_smtpHost, _smtpPort);
            smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);
        }
    }
}
