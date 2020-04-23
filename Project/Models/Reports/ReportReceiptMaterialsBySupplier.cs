using Project.Models.ReferenceInformation;
using System;

namespace Project.Models.Reports
{
    public class ReportReceiptMaterialsBySupplier
    {
        public Supplier Supplier { get; set; }

        public Material Material { get; set; }

        public int Count { get; set; }

        public DateTime DocumentDate { get; set; }

        public int DocumentNumber { get; set; }
    }
}
