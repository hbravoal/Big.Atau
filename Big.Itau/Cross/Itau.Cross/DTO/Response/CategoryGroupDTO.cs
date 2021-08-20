using System.Collections.Generic;

namespace Itau.Common.DTO.Response
{
    public class CategoryGroupDTO
    {
        public short Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Orden de redención del grupo
        /// </summary>
        public int? RedemptionOrder { get; set; }

        public string IconName { get; set; }
        public string Tooltip { get; set; }

        //Estado en la tabla si el grupo esta activo
        public bool Active { get; set; }

        public int ProgramId { get; set; }

        /// <summary>
        /// Estado del Grupo segun la acumulacion del cliente (Completa  o Aún le falta)
        /// </summary>
        public bool IsComplete { get; set; }

        
        /// <summary>
        /// Valor Acumulado en el grupo
        /// </summary>
        public decimal AvailableGroup { get; set; }

        /// <summary>
        /// Cuánto le falta para cumplir el grupo
        /// </summary>
        public decimal LacksMoneyGroup { get; set; }

        /// <summary>
        /// Meta mínima para el grupo.
        /// </summary>
        public decimal? MoneyGoalGroup { get; set; }

        /// <summary>
        /// Porcentaje de avance del grupo
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Lista de categorías asociadas al grupo.
        /// </summary>

        public List<CategoryDTO> Categories { get; set; }
    }
}