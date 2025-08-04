namespace CrudSenac.Domain.Dto
{
    public class ProdutoDto
    {
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Tipo { get; set; }
        public float Preco { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public UsuarioResumoDto Usuario { get; set; }
    }
}
