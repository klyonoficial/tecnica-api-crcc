using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Domain.Dtos;
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

        [Column("status")]
        public string? Status { get; set; }

        [Column("observacao")]
        public string? Observacao { get; set; }

        [InverseProperty("RotaCobranca")]
        public virtual ICollection<Cobranca>? Cobrancas { get; set; }


        public RotaCobranca(
            DateTime data,
            long? cobradorId,
            List<Cobranca>? cobrancas)
        {
            Assegure.NaoNulo(data, "A data deve ser informada.");
            Assegure.Que(cobradorId > 0, "O cobrador deve ser informado.");
            Assegure.NaoNulo(cobrancas, "A lista de cobranças deve ser informada.");
            Assegure.Que(cobrancas.Count > 0, "A lista de cobranças não pode estar vazia.");

            Data = data;
            CobradorId = cobradorId.Value;
            Cobrancas = cobrancas;
            Status = nameof(DomainEnums.RotaCobrancaStatus.PEND);
        }

        public RotaCobranca() { }

        public void Aprovar(long aprovadorId, string? observacao = null)
        {
            Assegure.Que(aprovadorId > 0, "O aprovador deve ser informado.");
            AprovadorId = aprovadorId;
            Observacao = observacao;
            Status = nameof(DomainEnums.RotaCobrancaStatus.APROV);
        }

        public void Reprovar(long reprovadorId, string? observacao)
        {
            Assegure.Que(reprovadorId > 0, "O reprovador deve ser informado.");
            AprovadorId = reprovadorId;
            Observacao = observacao;
            Status = nameof(DomainEnums.RotaCobrancaStatus.REPR);
        }

        public void AdicionarCobranca(long parcelaId)
        {
            Assegure.Que(parcelaId > 0, "A parcela deve ser informada.");

            if (Cobrancas == null)
                Cobrancas = new List<Cobranca>();

            var cobranca = new Cobranca(parcelaId);
            Cobrancas.Add(cobranca);
        }

        internal void PagarParcela(long parcelaId, PagamentoParcelaDto pagamentoDto)
        {
            Assegure.Que(parcelaId > 0, "A parcela deve ser informada.");

            var cobranca = Cobrancas?.FirstOrDefault(c => c.ParcelaId == parcelaId);
            Assegure.NaoNulo(cobranca, "Cobrança não encontrada.");

            var parcelaRemanejada = cobranca.Parcela.InformarPagamento(pagamentoDto);
            if (parcelaRemanejada != null)
            {
                // Adiciona a parcela remanejada à cobrança
                var novaCobranca = new Cobranca(parcelaRemanejada.Id);
                Cobrancas.Add(novaCobranca);
            }
        }

        internal void RemoverParcela(long parcelaId)
        {
            Assegure.Que(parcelaId > 0, "A parcela deve ser informada.");

            var cobranca = Cobrancas?.FirstOrDefault(c => c.ParcelaId == parcelaId);
            Assegure.NaoNulo(cobranca, "Cobrança não encontrada.");

            Cobrancas.Remove(cobranca);
        }
    }
}
