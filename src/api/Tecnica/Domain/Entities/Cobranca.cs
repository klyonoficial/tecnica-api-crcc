using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("cobranca")]
    public class Cobranca
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("rota_cobranca_id")]
        public long RotaCobrancaId { get; set; }

        [Column("parcela_id")]
        public long ParcelaId { get; set; }

        public Cobranca(long rotaCobrancaId, long parcelaId)
        {
            Assegure.Que(rotaCobrancaId > 0, "A rota de cobrança deve ser informada.");
            Assegure.Que(parcelaId > 0, "A parcela deve ser informada.");
            RotaCobrancaId = rotaCobrancaId;
        }

        // Construtor vazio para o Entity Framework
        public Cobranca() { }

        [ForeignKey("RotaCobrancaId")]
        [InverseProperty("Cobrancas")]
        public virtual RotaCobranca? RotaCobranca { get; set; }

        [ForeignKey("ParcelaId")]
        [InverseProperty("Cobrancas")]
        public virtual Parcela? Parcela { get; set; }
    }
}
