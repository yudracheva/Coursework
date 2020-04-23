using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports
{
    public class ReportsComponent : ComponentBase
    {
        public const string DATE_TO_PAGE_STRING_FORMAT = "yyyy-MM-ddTHH:mm";

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;
    }
}
