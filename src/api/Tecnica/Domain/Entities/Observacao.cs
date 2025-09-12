using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("observacao")]
    public class Observacao
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("entidade")]
        public string Entidade { get; set; }

        [Column("entidade_id")]
        public long EntidadeId { get; set; }

        [Column("texto")]
        public string Texto { get; set; }

        public Observacao(string entidade, long entidadeId, string texto)
        {
            Assegure.NaoNulo(entidade, "A entidade deve ser informada.");
            Assegure.Que(entidadeId > 0, "O id da entidade deve ser informado.");
            Assegure.NaoNulo(texto, "O texto deve ser informado.");
            Entidade = entidade;
            EntidadeId = entidadeId;
            Texto = texto;
        }

        // Construtor vazio para o Entity Framework
        public Observacao()
        {
            Entidade = string.Empty;
            Texto = string.Empty;
        }
    }
}
