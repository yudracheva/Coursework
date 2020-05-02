using Microsoft.AspNetCore.Components;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports.ReferencesAboutPaymentsWithSuppliers
{
    public partial class ReferencesAboutPaymentsWithSuppliersPage
    {
        [Parameter]
        public int SupplierId { get; set; }

        [Parameter]
        public DateTime BeginDate { get; set; }

        [Parameter]
        public DateTime EndDate { get; set; }

        protected int selectedSupplier;

        protected string selectedBeginDate;

        protected string selectedEndDate;

        protected bool showIFrame;

        protected List<Supplier> suppliers;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                suppliers = DatabaseProvider.GetSuppliers();
                selectedSupplier = SupplierId;
                if (BeginDate == DateTime.MinValue)
                    BeginDate = DateTime.Now.AddMonths(-1);

                if (EndDate == DateTime.MinValue)
                    EndDate = DateTime.Now.AddDays(1);

                selectedBeginDate = BeginDate.ToString(DATE_TO_PAGE_STRING_FORMAT);
                selectedEndDate = EndDate.ToString(DATE_TO_PAGE_STRING_FORMAT);

                UpdateData();
            }
        }

        private void UpdateData()
        {
            isLoad = false;
            try
            {
                if (BeginDate != DateTime.MinValue && EndDate != DateTime.MinValue && selectedSupplier != 0m)
                {
                    showIFrame = true;
                    NavigationManager.NavigateTo($"/references-about-payments-with-suppliers/{SupplierId}/{BeginDate.ToString("yyyy-MM-dd")}/{EndDate.ToString("yyyy-MM-dd")}");
                }
                else
                {
                    showIFrame = false;
                    NavigationManager.NavigateTo($"/references-about-payments-with-suppliers/{BeginDate.ToString("yyyy-MM-dd")}/{EndDate.ToString("yyyy-MM-dd")}");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Ну удалось сформировать справку. {ex.Message}", Models.MessageType.Error);
            }

            isLoad = true;

            StateHasChanged();
        }

        protected void ChangeEndDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            EndDate = date;
            selectedEndDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);

            UpdateData();
        }

        protected void ChangeBeginDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            BeginDate = date;
            selectedBeginDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);

            UpdateData();
        }

        protected void ChangeSelectedSupplier(ChangeEventArgs changeEventArgs)
        {
            selectedSupplier = Convert.ToInt32(changeEventArgs.Value);
            SupplierId = selectedSupplier;

            UpdateData();
        }
    }
}
