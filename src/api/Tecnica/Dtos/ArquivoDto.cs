namespace Tecnica.Domain.Dtos
{
    public class ArquivoDto
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
    }
}
