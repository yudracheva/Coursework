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

        protected bool isLoad;
        protected PaymentRequest document;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;
        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                document = DatabaseProvider.GetPaymentRequest(Id);
                selectedSupplier = document.Supplier?.Id ?? 0;
            }
            else
            {
                document = new PaymentRequest();
            }

            suppliers = DatabaseProvider.GetSuppliers();

            isLoad = true;
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

                DatabaseProvider.SavePaymentRequest(document);
                NavigationManager.NavigateTo("/receipt-of-materials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
