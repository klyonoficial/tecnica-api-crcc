using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("fabricante")]
    public class Fabricante
    {
        [Key]
        public long Id { get; private set; }

        [Column("nome")]
        public string Nome { get; private set; }

        [InverseProperty("Fabricante")]
        public virtual ICollection<Produto> Produtos { get; set; }

        private Fabricante() { }

        public Fabricante(string nome)
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
