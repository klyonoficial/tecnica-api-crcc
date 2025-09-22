namespace Tecnica.Domain.Dtos
{
    public class RotaCobrancaAprovacaoDto
    {
        public long AprovadorId { get; set; }
        public bool Aprovado { get; set; }
        public string? Observacao { get; set; }
    }
}