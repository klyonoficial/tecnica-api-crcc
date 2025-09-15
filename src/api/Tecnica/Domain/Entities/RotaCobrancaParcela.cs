using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("rota_cobranca_parcela")]
    public class RotaCobrancaParcela
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("rota_cobranca_id")]
        public long RotaCobrancaId { get; set; }

        [Column("parcela_id")]
        public long ParcelaId { get; set; }

        public RotaCobrancaParcela(long rotaCobrancaId, long parcelaId)
        {
            Assegure.Que(rotaCobrancaId > 0, "A rota de cobrança deve ser informada.");
            Assegure.Que(parcelaId > 0, "A parcela deve ser informada.");
            RotaCobrancaId = rotaCobrancaId;
            ParcelaId = parcelaId;
        }

        // Construtor vazio para o Entity Framework
        public RotaCobrancaParcela() { }

        [ForeignKey("RotaCobrancaId")]
        [InverseProperty("RotaCobrancaParcelas")]
        public virtual RotaCobranca? RotaCobranca { get; set; }

        [ForeignKey("ParcelaId")]
        [InverseProperty("RotaCobrancaParcelas")]
        public virtual VendaParcela? VendaParcela { get; set; }
    }
}
