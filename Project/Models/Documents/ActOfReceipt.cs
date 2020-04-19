using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Models.Documents
{
    /// <summary>
    /// Документ "Поступление материалов"
    /// </summary>
    public class ActOfReceipt : Document
    {
        /// <summary>
        /// Заказ поставщику
        /// </summary>
        public OrdersToSuppliers Order { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Список материалов
        /// </summary>
        public List<LineOfMaterials> Materials { get; set; } = new List<LineOfMaterials>();
    }
}
