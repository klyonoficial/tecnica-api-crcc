namespace Tecnica.Domain.Dtos
{
    public class RotaCobrancaDto
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public long CobradorId { get; set; }
        public long? AprovadorId { get; set; }
        public decimal TotalReceber { get; set; }
        public decimal? TotalRecebido { get; set; }
    }
}
