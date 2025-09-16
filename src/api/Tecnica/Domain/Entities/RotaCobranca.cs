using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("rota_cobranca")]
    public class RotaCobranca
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [Column("cobrador_id")]
        public long CobradorId { get; set; }

        [Column("aprovador_id")]
        public long? AprovadorId { get; set; }

        [Column("total_receber")]
        public decimal TotalReceber { get; set; }

        [Column("total_recebido")]
        public decimal? TotalRecebido { get; set; }

        [InverseProperty("RotaCobranca")]
        public virtual ICollection<Cobranca>? Cobrancas { get; set; }

        public RotaCobranca(
            DateTime data,
            long cobradorId,
            decimal totalReceber)
        {
            Assegure.NaoNulo(data, "A data deve ser informada.");
            Assegure.Que(cobradorId > 0, "O cobrador deve ser informado.");
            Assegure.Que(totalReceber > 0, "O total a receber deve ser informado.");
            Data = data;
            CobradorId = cobradorId;
            TotalReceber = totalReceber;
        }

        // Construtor vazio para o Entity Framework
        public RotaCobranca() { }
    }
}
