using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Models.Documents
{
    /// <summary>
    /// Документ "Заказ поставщику"
    /// </summary>
    public class OrdersToSuppliers : Document
    {
        /// <summary>
        /// Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Список материалов
        /// </summary>
        public List<LineOfMaterials> Materials { get; set; }
    }
}
