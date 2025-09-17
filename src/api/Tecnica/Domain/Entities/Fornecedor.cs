using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("fornecedor")]
    public class Fornecedor
    {
        [Key]
        public long Id { get; private set; }

        [Column("nome")]
        public string Nome { get; private set; }

        [InverseProperty("Fornecedor")]
        public virtual ICollection<Produto> Produtos { get; set; }

        private Fornecedor() { }

        public Fornecedor(string nome)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Nome = nome;
        }

        public void Atualizar(string nome)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Nome = nome;
        }
    }
}
