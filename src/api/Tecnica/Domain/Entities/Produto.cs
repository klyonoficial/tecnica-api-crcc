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

        [Column("fabricante_id")]
        public long FabricanteId { get; private set; }

        [ForeignKey("FabricanteId")]
        [InverseProperty("Produtos")]
        public Fabricante Fabricante { get; private set; }

        private Produto() { }

        public Produto(string nome, long fabricanteId)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.Que(fabricanteId > 0, "O fabricante deve ser informado.");

            Nome = nome;
            FabricanteId = fabricanteId;
        }
        public void Atualizar(string nome, long fabricanteId)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.Que(fabricanteId > 0, "O fabricante deve ser informado.");

            Nome = nome;
            FabricanteId = fabricanteId;
        }
    }
}
