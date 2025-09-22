using System;

namespace Tecnica.Dtos
{
    public class CobrancaPorDataDto
    {
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
    }
}