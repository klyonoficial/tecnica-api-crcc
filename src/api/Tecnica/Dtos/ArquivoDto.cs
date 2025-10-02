namespace Tecnica.Domain.Dtos
{
    public class ArquivoDto
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }

        public Tecnica.Domain.Entities.Arquivo ToEntity()
        {
            var entidade = new Tecnica.Domain.Entities.Arquivo(Tipo, Nome);
            if (Id > 0) entidade.Id = Id;
            return entidade;
        }
    }
}
