using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Domain.Dtos;
using Tecnica.Infra;
using static Tecnica.Domain.Entities.DomainEnums;

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
            decimal valor)
        {
            Assegure.NaoNulo(dataVencimento, "A data de vencimento deve ser informada.");
            Assegure.Que(valor > 0, "O valor deve ser informado.");

            DataVencimento = dataVencimento;
            Valor = valor;
            Status = ParcelaStatus.ABT.ToString();
        }

        public Parcela()
        {
        }

        public Parcela InformarPagamento(PagamentoParcelaDto pagamento)
        {
            if (pagamento.NaoPagou)
            {
                Assegure.Que(pagamento.DataRemanejamento.HasValue, "A data de remanejamento deve ser informada.");

                Status = ParcelaStatus.NPAG.ToString();
                DataRemanejamento = pagamento.DataRemanejamento;
                DataVencimento = pagamento.DataRemanejamento.Value;
                return null;
            }
            else
            {
                Assegure.Que(pagamento.FormaPagamentoId > 0, "A forma de pagamento deve ser informada.");
                Assegure.Que(pagamento.ValorPago > 0, "O valor pago deve ser informado.");

                FormaPagamentoId = pagamento.FormaPagamentoId;
                ValorPago = pagamento.ValorPago;
                DataPagamento = DateTime.Now;

                if (pagamento.ValorPago < Valor)
                {
                    Assegure.Que(pagamento.DataRemanejamento.HasValue, "A data de remanejamento deve ser informada.");
                    Status = ParcelaStatus.PARC.ToString();
                    DataRemanejamento = pagamento.DataRemanejamento;
                    var parcelaRemanejada = new Parcela(
                        pagamento.DataRemanejamento.Value,
                        Valor - pagamento.ValorPago);
                    Venda.AdicionarParcela(parcelaRemanejada);
                    return parcelaRemanejada;
                }
                else
                {
                    Status = ParcelaStatus.PAG.ToString();
                    return null;
                }
            }
        }
    }
}
