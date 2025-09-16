using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("rota_entrega")]
    public class RotaEntrega
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [InverseProperty("RotaEntrega")]
        public virtual ICollection<Entrega>? Entregas { get; set; }

        public RotaEntrega(DateTime data)
        {
            Assegure.NaoNulo(data, "A data deve ser informada.");
            Data = data;
        }

        // Construtor vazio para o Entity Framework
        public RotaEntrega() { }
    }
}
