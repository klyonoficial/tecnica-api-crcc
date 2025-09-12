using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("rota_entrega_venda")]
    public class RotaEntregaVenda
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("rota_entrega_id")]
        public long RotaEntregaId { get; set; }

        [Column("venda_id")]
        public long VendaId { get; set; }

        [Column("status")]
        public string Status { get; set; }

        public RotaEntregaVenda(long rotaEntregaId, long vendaId)
        {
            Assegure.Que(rotaEntregaId > 0, "A rota de entrega deve ser informada.");
            Assegure.Que(vendaId > 0, "A venda deve ser informada.");

            RotaEntregaId = rotaEntregaId;
            VendaId = vendaId;
            Status = null;
        }

        // Construtor vazio para o Entity Framework
        public RotaEntregaVenda()
        {

        }
    }
}
