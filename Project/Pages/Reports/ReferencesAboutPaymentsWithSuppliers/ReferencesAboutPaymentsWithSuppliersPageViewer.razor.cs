using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Project.Pages.Reports.ReferencesAboutPaymentsWithSuppliers
{
    public class ReferencesAboutPaymentsWithSuppliersPageViewerIndex : ComponentBase
    {
        [Parameter]
        public int SupplierId { get; set; }

        [Parameter]
        public DateTime BeginDate { get; set; }

        [Parameter]
        public DateTime EndDate { get; set; }

        protected List<object> lines;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            //throw new NotImplementedException();
        }

        public string GetTodayDate()
        {
            var date = DateTime.Today.ToString("f", CultureInfo.CreateSpecificCulture("ru-RU"));
            return date;
        }
    }
}
