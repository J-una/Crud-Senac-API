namespace CrudSenac.Domain.Dto
{
    public class LogintDto
    {
        public string Cpf { get; set; }
        public string Senha { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Nome { get; set; }
        public string Perfil { get; set; }
        public Guid IdUsuario { get; set; }
    }
}
