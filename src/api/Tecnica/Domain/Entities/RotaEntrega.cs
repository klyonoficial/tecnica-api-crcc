using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;
using Tecnica.Infra.Migrations;
using static Tecnica.Domain.Entities.DomainEnums;

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
        public virtual ICollection<Entrega> Entregas { get; set; }

        public RotaEntrega(DateTime data, ICollection<Entrega> entregas)
        {
            Assegure.NaoNulo(data, "A data deve ser informada.");
            Assegure.NaoNulo(entregas, "A lista de entregas deve ser informada.");
            Data = data;
            Entregas = entregas;
        }

        public void AddEntrega(Entrega entrega)
        {
            Assegure.NaoNulo(entrega, "A entrega deve ser informada.");
            Entregas.Add(entrega);
        }

        internal void RemoverEntrega(Entrega entrega)
        {
            Assegure.NaoNulo(entrega, "A entrega deve ser informada.");
            Entregas.Remove(entrega);
        }

        internal void AtualizarStatusEntrega(long vendaId, string status)
        {
            Assegure.NaoNulo(status, "O status deve ser informado.");

            var entrega = Entregas.FirstOrDefault(e => e.VendaId == vendaId);
            Assegure.NaoNulo(entrega, "A entrega para a venda informada não foi encontrada.");
            Assegure.Que(entrega.Status == EntregaStatus.ENTR.ToString()
           || entrega.Status == EntregaStatus.NENTR.ToString(), "Status da entrega deve ser ENTR ou NENTR.");

            entrega.Status = Enum.Parse<EntregaStatus>(status).ToString();

            if (entrega.Status == EntregaStatus.ENTR.ToString() && entrega.Venda != null)
                entrega.Venda.Entregue();

        }

        // Construtor vazio para o Entity Framework
        public RotaEntrega() { }
    }
}
