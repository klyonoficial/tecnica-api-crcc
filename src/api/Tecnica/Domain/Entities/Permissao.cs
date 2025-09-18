using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecnica.Domain.Entities
{
    [Table("permissao")]
    public class Permissao
    {
        [Key]
        [Column("id")]
        public long Id { get; private set; }

        [Column("componente_id")]
        public long ComponenteId { get; private set; }

        [Column("usuario_id")]
        public long UsuarioId { get; private set; }

        [ForeignKey("ComponenteId")]
        public virtual Componente? Componente { get; private set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; private set; }

        public Permissao(long componenteId, long usuarioId)
        {
            ComponenteId = componenteId;
            UsuarioId = usuarioId;
        }
    }
}