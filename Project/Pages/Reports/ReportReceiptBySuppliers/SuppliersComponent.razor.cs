using Microsoft.AspNetCore.Components;
using Project.Models.ReferenceInformation;
using Project.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports.ReportReceiptBySuppliers
{
    public partial class SuppliersComponent
    {
        [Parameter]
        public List<ReportReceiptMaterialsBySupplier> Materials { get; set; }

        [Parameter]
        public Supplier Supplier { get; set; }

        protected bool show;
    }
}
