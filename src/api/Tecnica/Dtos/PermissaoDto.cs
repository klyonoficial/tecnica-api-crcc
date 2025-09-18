namespace Tecnica.Dtos
{
    public class PermissaoDto
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public string? Ordem { get; set; }
        public string? Descricao { get; set; }
        public string? Tipo { get; set; }
        public string? Codigo { get; set; }
        public bool PossuiPermissao { get; set; }
    }
}