namespace CrudSenac.Domain.Dto
{
    public class ClienteDto
    {
        public Guid IdCliente { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public UsuarioResumoDto Usuario { get; set; }
    }
}
