using System.ComponentModel.DataAnnotations;

namespace CrudSenac.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public Guid IdUsuario { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiracao { get; set; }
        public bool Ativo {  get; set; }
        public string Perfil { get; set; }
    }
}
