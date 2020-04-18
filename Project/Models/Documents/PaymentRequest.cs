using Project.Models.ReferenceInformation;

namespace Project.Models.Documents
{
    /// <summary>
    /// Документ "Платежное требование"
    /// </summary>
    public class PaymentRequest : Document
    {
        /// <summary>
        /// Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public decimal Sum { get; set; }
    }
}
