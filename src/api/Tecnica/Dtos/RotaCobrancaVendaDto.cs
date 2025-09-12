namespace Tecnica.Domain.Dtos
{
    public class RotaCobrancaVendaDto
    {
        public long Id { get; set; }
        public long RotaCobrancaId { get; set; }
        public long VendaId { get; set; }
        public long ParcelaId { get; set; }
    }
}
