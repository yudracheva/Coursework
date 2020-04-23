using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.PaymentRequestPages
{
    public partial class PaymentRequestPage
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected PaymentRequest document;
        protected bool isLoad;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;
        protected string selectedDate;

        protected void ChangeDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);
        }

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                document = DatabaseProvider.GetPaymentRequest(Id);
                selectedSupplier = document.Supplier?.Id ?? 0;
                selectedDate = document.CreatedDate.ToString(DATE_TO_PAGE_STRING_FORMAT);
            }
            else
            {
                document = new PaymentRequest();
                selectedDate = DateTime.Now.ToString(DATE_TO_PAGE_STRING_FORMAT);
            }

            suppliers = DatabaseProvider.GetSuppliers();

            isLoad = true;

            StateHasChanged();
        }

        protected void Save()
        {
            try
            {
                if (selectedSupplier != 0)
                {
                    var category = suppliers.Where(d => d.Id.Equals(selectedSupplier)).FirstOrDefault();
                    document.Supplier = category;
                }
                else
                {
                    document.Supplier = null;
                }

                document.CreatedDate = DateTime.Parse(selectedDate);

                DatabaseProvider.SavePaymentRequest(document);
                NavigationManager.NavigateTo("/payment-requests");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
