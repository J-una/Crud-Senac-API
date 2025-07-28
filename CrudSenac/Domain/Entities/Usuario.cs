using System.ComponentModel.DataAnnotations;

namespace CrudSenac.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public Guid IdUsuario { get; set; }

        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo {  get; set; }
        //Alterar para uma chave estrangeira Perfil
        //public string Perfil { get; set; }
    }
}
