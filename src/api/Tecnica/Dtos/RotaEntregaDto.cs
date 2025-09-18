namespace Tecnica.Domain.Dtos
{
    public class RotaEntregaDto
    {
        public long? Id { get; set; }
        public DateTime Data { get; set; }
        public List<EntregaDto> Entregas { get; set; } = new List<EntregaDto>();
    }

    public class EntregaDto
    {
        public long? Id { get; set; }
        public long VendaId { get; set; }
        public string Status { get; set; } = string.Empty;
        public long ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string? EnderecoBairro { get; set; }
        public string? EnderecoUF { get; set; }
        public string? EnderecoNumero { get; set; }
        public string? EnderecoComplemento { get; set; }
    }
}