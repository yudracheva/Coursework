using Microsoft.AspNetCore.Components;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Reports.ReportsOnPaymentsBySupplier
{
    public partial class ReportsOnPaymentsBySupplierPage
    {
        protected List<PaymentRequest> lines;
        protected string selectedBeginDate;
        protected string selectedEndDate;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                suppliers = DatabaseProvider.GetSuppliers();
                selectedEndDate = DateTime.Today.AddDays(1).ToString(DATE_TO_PAGE_STRING_FORMAT);
                selectedBeginDate = DateTime.Today.AddMonths(-1).ToString(DATE_TO_PAGE_STRING_FORMAT);

                UpdateData();
            }
        }

        private void UpdateData()
        {
            isLoad = false;
            try
            {
                lines = DatabaseProvider.GetPaymentsRequests();

                var beginDate = DateTime.Parse(selectedBeginDate);
                var endDate = DateTime.Parse(selectedEndDate);

                lines = lines.Where(d => d.CreatedDate > beginDate && d.CreatedDate < endDate).ToList();
                if (selectedSupplier != 0)
                    lines = lines.Where(d => d.Supplier.Id.Equals(selectedSupplier)).ToList();
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось загрузить данные для отчета. {ex.Message}", Models.MessageType.Error);
            }

            isLoad = true;

            StateHasChanged();
        }

        protected void ChangeEndDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedEndDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);

            UpdateData();
        }

        protected void ChangeBeginDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedBeginDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);

            UpdateData();
        }

        protected void ChangeSelectedSupplier(ChangeEventArgs changeEventArgs)
        {
            selectedSupplier = Convert.ToInt32(changeEventArgs.Value);

            UpdateData();
        }
    }
}
