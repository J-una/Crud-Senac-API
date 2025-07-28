using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace CrudSenac.Domain.Entities
{
    public class Produto
    {
        [Key]
        public Guid IdProduto { get; set; }


        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Tipo { get; set; }
        public float Preco { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get;set; }
        public bool Ativo { get; set; }
        public Guid IdUsuario { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; }
    }
}
