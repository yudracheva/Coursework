using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports
{
    public class ReportsComponent : DefaultComponentBase
    {
        public const string DATE_TO_PAGE_STRING_FORMAT = "yyyy-MM-ddTHH:mm";

        public const string DATE_TO_PAGE_TO_SHOW = "dd.MM.yyyy HH:mm";

        protected bool isLoad;
    }
}
