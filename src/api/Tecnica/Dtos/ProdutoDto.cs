namespace Tecnica.Domain.Dtos
{
    public class ProdutoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public long FabricanteId { get; set; }
        public string FabricanteNome { get; set; }
    }
}
