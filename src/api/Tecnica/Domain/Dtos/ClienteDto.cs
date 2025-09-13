// Duplicate DTO retained for legacy compatibility; original is in /Tecnica/Dtos. Disabled to avoid duplicate type definition.
#if false
using System;

namespace Tecnica.Domain.Dtos
{
    public class ClienteDto
    {
        public long Id { get; set; }
        public long VendedorId { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? Contato { get; set; }
        public string? TelefoneDdd { get; set; }
        public string? TelefoneNumero { get; set; }
        public int DiaCobranca { get; set; }
        public string? Status { get; set; }
        public DateTime? DataAprovou { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? DadosSerasa { get; set; }
    }
}
#endif