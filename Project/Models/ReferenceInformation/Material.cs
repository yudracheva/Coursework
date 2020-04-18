namespace Project.Models.ReferenceInformation
{
    /// <summary>
    /// Материал
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Идентификатор материала
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование материала
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описаниае
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Поставщик материала
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Признак приостановки поставки
        /// </summary>
        public bool DeliveriesStopped { get; set; }

        /// <summary>
        /// Категория материала
        /// </summary>
        public MaterialCategory Category { get; set; }
    }
}
