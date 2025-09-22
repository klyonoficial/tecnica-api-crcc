namespace Tecnica.Domain.Dtos
{
    public class PagamentoParcelaDto
    {
        public long FormaPagamentoId { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime? DataRemanejamento { get; set; }
        public bool NaoPagou { get; set; }
        public string? Observacao { get; set; }
    }
}