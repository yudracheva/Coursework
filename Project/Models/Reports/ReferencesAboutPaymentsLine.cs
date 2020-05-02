using Project.Models.ReferenceInformation;
using System;

namespace Project.Models.Reports
{
    public class ReferencesAboutPaymentsLine
    {
        /// <summary>
        /// Порядковый номер строки
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Числится задолжность "за что"
        /// </summary>
        public string OutstandingBalanceAbout { get; set; }

        /// <summary>
        /// Числится задолжность "дата начала"
        /// </summary>
        public DateTime OutstandingBalanceDate { get; set; }

        /// <summary>
        /// Дебиторская задолжность
        /// </summary>
        public decimal DebitSum { get; set; }

        /// <summary>
        /// Кредиторская задолжность
        /// </summary>
        public decimal CreditSum { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime DocumentDate { get; set; }
    }
}

