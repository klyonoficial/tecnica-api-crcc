using System;

namespace Tecnica.Domain.Dtos
{
    public class ClienteResultDto
    {
        public long Id { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? Contato { get; set; }
        public string? Telefone { get; set; }
        public string? Status { get; set; }
    }
}
