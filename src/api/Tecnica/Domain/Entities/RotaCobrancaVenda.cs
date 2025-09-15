using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("rota_cobranca_venda")]
    public class RotaCobrancaVenda
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("rota_cobranca_id")]
        public long RotaCobrancaId { get; set; }

        [Column("venda_id")]
        public long VendaId { get; set; }

        [Column("parcela_id")]
        public long ParcelaId { get; set; }

        public RotaCobrancaVenda(long rotaCobrancaId, long vendaId, long parcelaId)
        {
            Assegure.Que(rotaCobrancaId > 0, "A rota de cobrança deve ser informada.");
            Assegure.Que(vendaId > 0, "A venda deve ser informada.");
            Assegure.Que(parcelaId > 0, "A parcela deve ser informada.");
            RotaCobrancaId = rotaCobrancaId;
            VendaId = vendaId;
            ParcelaId = parcelaId;
        }

        // Construtor vazio para o Entity Framework
        public RotaCobrancaVenda() { }

        [ForeignKey("RotaCobrancaId")]
        [InverseProperty("RotaCobrancaVendas")]
        public virtual RotaCobranca? RotaCobranca { get; set; }

        [ForeignKey("VendaId")]
        [InverseProperty("RotaCobrancaVendas")]
        public virtual Venda? Venda { get; set; }

        [ForeignKey("ParcelaId")]
        [InverseProperty("RotaCobrancaVendas")]
        public virtual VendaParcela? VendaParcela { get; set; }
    }
}
