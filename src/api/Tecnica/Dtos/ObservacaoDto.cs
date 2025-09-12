namespace Tecnica.Domain.Dtos
{
    public class ObservacaoDto
    {
        public long Id { get; set; }
        public string Entidade { get; set; }
        public long EntidadeId { get; set; }
        public string Texto { get; set; }
    }
}
