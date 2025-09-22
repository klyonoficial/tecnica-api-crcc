namespace Tecnica.Domain.Dtos
{
    public class RotaCobrancaDto
    {
        public long? Id { get; set; }
        public DateTime Data { get; set; }
        public long? CobradorId { get; set; }
        public string CobradorNome { get; set; } = string.Empty;
        public List<CobrancaDto> Cobrancas { get; set; } = new List<CobrancaDto>();
    }

    public class CobrancaDto
    {
        public long Id { get; set; }
        public long ParcelaId { get; set; }
        public long VendaId { get; set; }
        public long ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = string.Empty;
        public string EnderecoLogradouro { get; set; } = string.Empty;
        public string EnderecoNumero { get; set; } = string.Empty;
        public string EnderecoComplemento { get; set; } = string.Empty;
        public string EnderecoBairro { get; set; } = string.Empty;
        public string EnderecoCidade { get; set; } = string.Empty;
        public string EnderecoUf { get; set; } = string.Empty;
        public string EnderecoCep { get; set; } = string.Empty;
    }
}