using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;
using static Tecnica.Domain.Entities.DomainEnums;

namespace Tecnica.Domain.Entities
{
    [Table("entrega")]
    public class Entrega
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

        [ForeignKey("RotaEntregaId")]
        [InverseProperty("Entregas")]
        public virtual RotaEntrega? RotaEntrega { get; set; }

        [ForeignKey("VendaId")]
        [InverseProperty("Entregas")]
        public virtual Venda? Venda { get; set; }

        public Entrega(long vendaId)
        {
            Assegure.Que(vendaId > 0, "A venda deve ser informada.");

            VendaId = vendaId;
            VendaId = vendaId;
            Status = EntregaStatus.PEND.ToString();
        }

        // Construtor vazio para o Entity Framework
        public Entrega()
        {

        }
    }
}
