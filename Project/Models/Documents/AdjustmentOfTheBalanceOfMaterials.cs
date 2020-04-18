using System.Collections.Generic;

namespace Project.Models.Documents
{
    /// <summary>
    /// Документ "Корректировка остатков"
    /// </summary>
    public class AdjustmentOfTheBalanceOfMaterials : Document
    {
        /// <summary>
        /// Список материалов
        /// </summary>
        public List<LineOfMaterials> Materials { get; set; }
    }
}
