using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Fornecedor Fornecedor { get; private set; }

        // Construtor requerido pelo Entity Framework
        private Produto() { }

        public Produto(string nome, Fornecedor fornecedor)
        {
            Asssegure.NaoNullo(nome, "O nome deve ser informado.");
            Asssegure.NaoNullo(fornecedor, "O fornecedor deve ser informado.");

            Nome = nome;
            Fornecedor = fornecedor;
            FornecedorId = fornecedor.Id;
        }
    }
}
