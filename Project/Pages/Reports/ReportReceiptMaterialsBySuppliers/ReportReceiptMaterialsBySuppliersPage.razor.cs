using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.ReferenceInformation;
using Project.Models.Documents;
using Project.Models.Reports;
using NLog.Filters;

namespace Project.Pages.Reports.ReportReceiptMaterialsBySuppliers
{
    public partial class ReportReceiptMaterialsBySuppliersPage
    {
        [Parameter]
        public int SupplierId { get; set; }

        protected List<ReportReceiptMaterialsBySupplier> lines;
        protected string selectedBeginDate;
        protected string selectedEndDate;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                suppliers = DatabaseProvider.GetSuppliers();
                selectedEndDate = DateTime.Today.ToString(DATE_TO_PAGE_STRING_FORMAT);
                selectedBeginDate = DateTime.Today.AddMonths(-1).ToString(DATE_TO_PAGE_STRING_FORMAT);

                UpdateData();
            }
        }

        private void UpdateData()
        {
            isLoad = false;

            lines = DatabaseProvider.GetReportReceiptMaterialsBySupplier();

            var beginDate = DateTime.Parse(selectedBeginDate);
            var endDate = DateTime.Parse(selectedEndDate);
            if (selectedSupplier != 0)
                lines = lines.Where(d => d.Supplier.Id == selectedSupplier).ToList();

            lines = lines.Where(d => d.DocumentDate > beginDate && d.DocumentDate < endDate).ToList();
            if (SupplierId != 0)
            {
                lines = lines.Where(d => (d.Supplier != null) && (d.Supplier.Id.Equals(SupplierId))).ToList();
                selectedSupplier = SupplierId;
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

        protected void Print()
        {

        }
    }
}
