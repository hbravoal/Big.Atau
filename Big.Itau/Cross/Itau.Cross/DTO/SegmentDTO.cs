using Itau.Common.DTO.Response;
using System;
using System.Collections.Generic;

namespace Itau.Common.DTO
{
    public class SegmentDTO
    {
        public Guid CustomerGuid { get; set; }
        public List<CategoryGroupDTO> ListCategoriesGroups { get; set; }

        /// <summary>
        /// limite de redenciones para el cliente true=alcanzo el limite y no puede redimir
        /// </summary>
        public bool RedemptionLimitReached { get; set; }

        /// <summary>
        /// Cantidad de grupos que necesita activar.
        /// </summary>
        public byte BillingGroupGoal { get; set; }

        /// <summary>
        /// Si el customer puede redimir
        /// </summary>
        public bool CanRedeem { get; set; }

        /// <summary>
        /// Si el customer tiene misiones activas
        /// </summary>
        public bool MisionsActive { get; set; }

        /// <summary>
        /// Límite de redenciones para el cliente.
        /// </summary>
        public int RedemptionLimit { get; set; }

        // Fecha de la última transacción en track log.
        public DateTime DateTransaction { get; set; }
    }
}