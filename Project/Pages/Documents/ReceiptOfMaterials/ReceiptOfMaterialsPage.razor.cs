using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.ReceiptOfMaterials
{
    public class ReceiptOfMaterialsPageIndex : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected ActOfReceipt document;
        protected bool isLoad;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                document = DatabaseProvider.GetActOfReceipt(Id);
                selectedSupplier = document.Supplier?.Id ?? 0;
            }
            else
            {
                document = new ActOfReceipt();
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

                DatabaseProvider.SaveActOfReceipt(document);
                NavigationManager.NavigateTo("/receipt-of-materials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }

    }
}
