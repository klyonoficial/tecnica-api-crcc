using System;

namespace Tecnica.Domain.Dtos
{
    public class ClienteResumoComprasDto
    {
        public long ClienteId { get; set; }
        public decimal TotalComprado { get; set; }
        public int QuantidadeVendas { get; set; }
        public DateTime? DataUltimaCompra { get; set; }
    }
}
