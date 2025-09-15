using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("produto")]
    public class Produto
    {
        [Key]
        public long Id { get; private set; }

        [Column("nome")]
        [Required]
        public string Nome { get; private set; }

        [Column("fornecedor_id")]
        public long FornecedorId { get; private set; }

        [ForeignKey("FornecedorId")]
        [InverseProperty("Produtos")]
        public Fornecedor Fornecedor { get; private set; }

        // Construtor requerido pelo Entity Framework
        private Produto() { }

        public Produto(string nome, Fornecedor fornecedor)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.NaoNulo(fornecedor, "O fornecedor deve ser informado.");

            Nome = nome;
            Fornecedor = fornecedor;
            FornecedorId = fornecedor.Id;
        }
    }
}
