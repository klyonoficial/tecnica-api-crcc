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

        private Produto() { }

        public Produto(string nome, long fornecedorId)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.Que(fornecedorId > 0, "O fornecedor deve ser informado.");

            Nome = nome;
            FornecedorId = fornecedorId;
        }
        public void Atualizar(string nome, long fornecedorId)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.Que(fornecedorId > 0, "O fornecedor deve ser informado.");

            Nome = nome;
            FornecedorId = fornecedorId;
        }
    }
}
