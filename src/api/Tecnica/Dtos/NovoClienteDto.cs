namespace Tecnica.Domain.Dtos
{
    public class NovoClienteDto
    {
        public long VendedorId { get; set; }
        public string CpfCnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Contato { get; set; }
        public string? TelefoneDdd { get; set; }
        public string? TelefoneNumero { get; set; }
        public int DiaCobranca { get; set; }
        public string? DadosSerasa { get; set; }
    }
}
