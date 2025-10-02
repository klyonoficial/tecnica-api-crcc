namespace Tecnica.Domain.Dtos
{
    public class EnderecoDto
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string? CodigoIbge { get; set; }
        public string? Bairro { get; set; }
        public string? UF { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Tipo { get; set; }

        // Converte o DTO para a entidade. ClienteId é passado explicitamente
        public Tecnica.Domain.Entities.Endereco ToEntity()
        {
            var entidade = new Tecnica.Domain.Entities.Endereco(Cep, Logradouro, CodigoIbge, Bairro, UF, Numero, Complemento, Tipo);
            if (Id > 0) entidade.Id = Id;
            return entidade;
        }
    }
}
