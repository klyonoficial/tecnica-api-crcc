namespace Tecnica.Domain.Dtos
{
    public class RotaEntregaVendaDto
    {
        public long Id { get; set; }
        public long RotaEntregaId { get; set; }
        public long VendaId { get; set; }
        public string Status { get; set; }
    }
}
