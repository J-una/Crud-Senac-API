namespace CrudSenac.Domain.Interfaces
{
    public interface IEmailService
    {

        Task EnviarEmailAsync(string para, string assunto, string corpo);
    }
}
