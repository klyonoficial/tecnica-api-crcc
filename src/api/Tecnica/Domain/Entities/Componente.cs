using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecnica.Domain.Entities
{
    [Table("componente")]
    public class Componente
    {
        [Key]
        [Column("id")]
        public long Id { get; private set; }

        [Column("codigo")]
        [MaxLength(200)]
        public string Codigo { get; private set; }

        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; private set; }

        [Column("descricao")]
        [MaxLength(255)]
        public string Descricao { get; private set; }

        [Column("tipo")]
        [MaxLength(1)]
        public string Tipo { get; private set; }

        [Column("ordem")]
        [MaxLength(20)]
        public string Ordem { get; private set; }

        [Column("ativo")]
        public bool Ativo { get; private set; }

        public Componente(string codigo, string nome, string tipo)
        {
            Codigo = codigo;
            Nome = nome;
            Tipo = tipo;
            Ativo = true;
        }
    }
}