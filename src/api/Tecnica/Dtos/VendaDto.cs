using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Dtos
{
    public class VendaDto
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Status { get; set; }
        public long ClienteId { get; set; }
        public long VendedorId { get; set; }
        public DateTime? DataEntrega { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? ValorEntrada { get; set; }
        public int DiaAcerto { get; set; }
        public int QuantidadeParcelas { get; set; }
        public string? Observacao { get; set; }
        public string? EnderecoCep { get; set; }
        public string? EnderecoCodigoIbge { get; set; }
        public string? EnderecoBairro { get; set; }
        public string? EnderecoUF { get; set; }
        public string? EnderecoNumero { get; set; }
        public string? EnderecoComplemento { get; set; }
        public string? EnderecoTipo { get; set; }
        public List<ItemDto> Produtos { get; set; } = new();
        public List<ParcelaDto> Parcelas { get; set; } = new();
        public string? EnderecoLogradouro { get; set; }
    }
}
