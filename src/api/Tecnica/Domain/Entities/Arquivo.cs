using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("arquivo")]
    public class Arquivo
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("cliente_id")]
        public long ClienteId { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        public Arquivo(string tipo, string nome)
        {
            Assegure.NaoNulo(tipo, "O tipo deve ser informado.");
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Tipo = tipo;
            Nome = nome;
        }

        // Construtor vazio para o Entity Framework
        public Arquivo()
        {
        }

        [ForeignKey("ClienteId")]
        [InverseProperty("Arquivos")]
        public Cliente? Cliente { get; set; }
    }
}
