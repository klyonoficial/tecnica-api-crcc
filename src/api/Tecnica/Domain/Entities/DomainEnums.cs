using System;

namespace Tecnica.Domain.Entities
{
    /// <summary>
    /// Centralizes domain enums used across the Tecnica project.
    /// </summary>
    public static class DomainEnums
    {
        public enum ClienteStatus
        {
            CAD,
            APROV,
            REPR,
            INAT
        }

        public enum VendaStatus
        {
            CAD,
            APROV,
            ARQ,
            EXP,
            ENTR,
            REPR
        }

        public enum ParcelaStatus
        {
            ABT,
            PAG,
            PARC,
            NPAG
        }

        public enum RotaEntregaVendaStatus
        {
            ENTR,
            NENTR
        }

        public enum RotaCobrancaStatus
        {
            CAD,
            APROV,

            REPR,
        }
    }
}
