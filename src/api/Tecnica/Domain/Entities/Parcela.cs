using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("parcela")]
    public class Parcela
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("venda_id")]
        public long VendaId { get; set; }

        [Column("data_vencimento")]
        public DateTime DataVencimento { get; set; }

        [Column("data_pagamento")]
        public DateTime? DataPagamento { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("valor_pago")]
        public decimal? ValorPago { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("data_remanejamento")]
        public DateTime? DataRemanejamento { get; set; }

        [Column("forma_pagamento_id")]
        public long? FormaPagamentoId { get; set; }

        [ForeignKey("VendaId")]
        [InverseProperty("Parcelas")]
        public virtual Venda? Venda { get; set; }

        [InverseProperty("Parcela")]
        public virtual ICollection<Cobranca>? Cobrancas { get; set; }

        public Parcela(
            DateTime dataVencimento,
            decimal valor,
            string status)
        {
            Assegure.NaoNulo(dataVencimento, "A data de vencimento deve ser informada.");
            Assegure.Que(valor > 0, "O valor deve ser informado.");

            DataVencimento = dataVencimento;
            Valor = valor;
            Status = status;
        }

        public Parcela()
        {

        }
    }
}
