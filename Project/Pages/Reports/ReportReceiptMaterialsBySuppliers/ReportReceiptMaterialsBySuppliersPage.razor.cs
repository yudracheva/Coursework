using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports.ReportReceiptMaterialsBySuppliers
{
    public partial class ReportReceiptMaterialsBySuppliersPage
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;


    }
}
