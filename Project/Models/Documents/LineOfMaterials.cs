using Project.Models.ReferenceInformation;

namespace Project.Models.Documents
{
    /// <summary>
    /// Строка материалов
    /// </summary>
    public class LineOfMaterials
    {
        /// <summary>
        /// Номер строки
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Материал
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// Выбранный материал
        /// </summary>
        public int SelectedMaterial { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum { get; set; }
    }
}
