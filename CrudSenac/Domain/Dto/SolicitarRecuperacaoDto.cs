namespace CrudSenac.Domain.Dto
{
    public class SolicitarRecuperacaoDto
    {
        public string Email { get; set; }
    }

    public class RedefinirSenhaDto
    {
        public string Token { get; set; }
        public string NovaSenha { get; set; }
    }
}
