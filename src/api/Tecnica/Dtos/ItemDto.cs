namespace Tecnica.Domain.Dtos
{
    public class ItemDto
    {
        public long Id { get; set; }
        public long VendaId { get; set; }
        public long ProdutoId { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string? NumerosSerie { get; set; }
    }
}
