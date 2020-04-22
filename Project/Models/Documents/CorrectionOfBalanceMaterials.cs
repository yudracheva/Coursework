using System.Collections.Generic;

namespace Project.Models.Documents
{
    /// <summary>
    /// Документ "Корректировка остатков"
    /// </summary>
    public class CorrectionOfBalanceMaterials : Document
    {
        /// <summary>
        /// Список материалов
        /// </summary>
        public List<LineOfMaterials> Materials { get; set; } = new List<LineOfMaterials>();
    }
}
