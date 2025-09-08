using System.ComponentModel.DataAnnotations;

namespace CrudSenac.Domain.Entities
{
    public class RecuperacaoSenha
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
        public bool Utilizado { get; set; }
    }
}
