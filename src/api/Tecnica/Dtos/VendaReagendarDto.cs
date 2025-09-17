using System;
using System.Collections.Generic;

namespace Tecnica.Domain.Dtos
{
    public class VendaReagendarDto
    {
        public long Id { get; set; }
        public DateTime NovaDataEntrega { get; set; }
        public string? Observacao { get; set; }
    }
}