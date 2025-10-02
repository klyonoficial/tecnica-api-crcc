using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("endereco")]
    public class Endereco
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("pessoa_id")]
        public long PessoaId { get; set; }

        [Column("cep")]
        public string Cep { get; set; }

        [Column("codigo_ibge")]
        public string? CodigoIbge { get; set; }

        [Column("logradouro")]
        public string? Logradouro { get; set; }

        [Column("bairro")]
        public string? Bairro { get; set; }

        [Column("UF")]
        public string? UF { get; set; }

        [Column("numero")]
        public string? Numero { get; set; }

        [Column("complemento")]
        public string? Complemento { get; set; }

        [Column("tipo")]
        public string? Tipo { get; set; }

        public Endereco(string cep, 
         string logradouro,
         string? codigoIbge, string? bairro, string? uf, string? numero, string? complemento, string? tipo)
        {
            Assegure.NaoNulo(cep, "O CEP deve ser informado.");
            Assegure.NaoNulo(logradouro, "O logradouro deve ser informado.");
            Assegure.NaoNulo(bairro, "O bairro deve ser informado.");
            Assegure.NaoNulo(uf, "A UF deve ser informada.");
            Assegure.NaoNulo(tipo, "O tipo deve ser informado.");

            Cep = cep;
            Logradouro = logradouro;
            CodigoIbge = codigoIbge;
            Bairro = bairro;
            UF = uf;
            Numero = numero;
            Complemento = complemento;
            Tipo = tipo;
        }

        // Construtor vazio para o Entity Framework
        public Endereco()
        {
            
        }

        [ForeignKey("PessoaId")]
        [InverseProperty("Enderecos")]
        public virtual Cliente? Cliente { get; set; }
    }
}
