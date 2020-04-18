namespace Project.Models
{
    /// <summary>
    /// Поставщик
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// Идентификатор поставщика (Заполняется по счетчику)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public string KPP { get; set; }

        /// <summary>
        /// Расчетный счет организации
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Банка
        /// </summary>
        public Bank Bank { get; set; }
    }
}
