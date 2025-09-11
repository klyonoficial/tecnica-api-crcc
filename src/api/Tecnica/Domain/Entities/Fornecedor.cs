using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecnica.Domain.Entities
{
    [Table("fornecedor")]
    public class Fornecedor
    {
        [Key]
        public long Id { get; private set; }

        [Column("nome")]
        [Required]
        public string Nome { get; private set; }

        private Fornecedor() { }

        public Fornecedor(string nome)
        {
            Asssegure.NaoNullo(nome, "O nome deve ser informado.");
            Nome = nome;
        }
    }
}
