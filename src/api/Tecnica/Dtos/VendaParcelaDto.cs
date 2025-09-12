namespace Tecnica.Domain.Dtos
{
    public class VendaParcelaDto
    {
        public long Id { get; set; }
        public long VendaId { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorPago { get; set; }
        public string Status { get; set; }
        public DateTime? DataRemanejamento { get; set; }
        public long? FormaPagamentoId { get; set; }
    }
}
