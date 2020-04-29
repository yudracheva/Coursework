using Microsoft.AspNetCore.Components;
using Project.Models.ReferenceInformation;
using Project.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports.ListsOfMaterialsOnTheWay
{
    public partial class MaterialComponent
    {
        [Parameter]
        public List<ReportListsOfMaterialsOnTheWay> Materials { get; set; }

        [Parameter]
        public ReportListsOfMaterialsOnTheWay reportListsOfMaterialsOnTheWay { get; set; }

        protected bool show;
    }
}
