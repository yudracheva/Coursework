using Project.Models.ReferenceInformation;
using System;

namespace Project.Models.Reports
{
    public class ReportListsOfMaterialsOnTheWay
    {
        public int SelectedMaterial { get; set; }

        public Material Material { get; set; }

        public int Count { get; set; }

        public DateTime DocumentDate { get; set; }

        public int DocumentNumber { get; set; }

        public Supplier Supplier { get; internal set; }

        public int SelectedSupplier { get; internal set; }
    }
}
