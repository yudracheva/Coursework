using Microsoft.AspNetCore.Components;
using Project.Models.ReferenceInformation;
using Project.Models.Reports;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Project.Pages.Reports.ReferencesAboutPayments
{
    public partial class ReferencesAboutPaymentsViewer
    {
        [Parameter]
        public int SupplierId { get; set; }

        [Parameter]
        public DateTime BeginDate { get; set; }

        [Parameter]
        public DateTime EndDate { get; set; }

        protected List<ReferencesAboutPaymentsLine> lines;

        protected bool isLoad;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            isLoad = false;
            try
            {
                lines = DatabaseProvider.GetReferencesAboutPaymentsInfo(SupplierId, BeginDate, EndDate);
            }
            catch (Exception ex)
            {
                ShowMessage($"Ну удалось сформировать справку. {ex.Message}", Models.MessageType.Error);
            }

            isLoad = true;

            StateHasChanged();
        }

        public string GetTodayDate()
        {
            var date = DateTime.Today.ToString("D", CultureInfo.CreateSpecificCulture("ru-RU"));
            return date;
        }

        public string GetInfoAboutSupplier(Supplier supplier)
        {
            return supplier?.OrganizationName ?? "";
        }
    }
}
