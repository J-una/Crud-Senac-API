namespace CrudSenac.Domain.Interfaces
{
    public interface IEmailInterface
    {
        Task EnviarEmailAsync(string para, string assunto, string mensagem);
    }
}
