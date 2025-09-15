using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("venda_produto")]
    public class VendaProduto
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("venda_id")]
        public long VendaId { get; set; }

        [Column("produto_id")]
        public long ProdutoId { get; set; }

        [Column("preco")]
        public decimal Preco { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Column("numeros_serie")]
        public string? NumerosSerie { get; set; }

        [ForeignKey("VendaId")]
        [InverseProperty("VendaProdutos")]
        public virtual Venda? Venda { get; set; }

        public VendaProduto(long vendaId, long produtoId, decimal preco, int quantidade, string? numerosSerie)
        {
            Assegure.Que(vendaId > 0, "A venda deve ser informada.");
            Assegure.Que(produtoId > 0, "O produto deve ser informado.");
            Assegure.Que(preco > 0, "O preço deve ser informado.");
            Assegure.Que(quantidade > 0, "A quantidade deve ser informada.");
            Assegure.NaoNulo(numerosSerie, "Os números de série devem ser informados.");

            VendaId = vendaId;
            ProdutoId = produtoId;
            Preco = preco;
            Quantidade = quantidade;
            NumerosSerie = numerosSerie;
        }

        // Construtor vazio para o Entity Framework
        public VendaProduto() { }
    }
}
