using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecnica.Domain.Entities
{
    [Table("cidade")]
    public class Cidade
    {
        [Key]
        [Column("id_cidade")]
        public long IdCidade { get; private set; }

        [Column("codigo_ibge")]
        [Required]
        [MaxLength(30)]
        public string CodigoIbge { get; private set; }

        [Column("nome")]
        [Required]
        [MaxLength(400)]
        public string Nome { get; private set; }

        [Column("uf")]
        [Required]
        [MaxLength(2)]
        public string Uf { get; private set; }

        [Column("status")]
        [Required]
        [MaxLength(4)]
        public string Status { get; private set; }

        // Construtor privado para EF
        private Cidade() { }
    }
}
