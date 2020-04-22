using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;

namespace Project.Pages.Documents.AdjustmentOfTheBalanceOfMaterials
{
    public partial class AdjustmentOfTheBalanceOfMaterialsPage
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected CorrectionOfBalanceMaterials document;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                document = DatabaseProvider.GetCorrectionOfBalanceMaterials(Id);
            }
            else
            {
                document = new CorrectionOfBalanceMaterials();
            }

            suppliers = DatabaseProvider.GetSuppliers();

            isLoad = true;
        }

        protected void Save()
        {
            try
            {
                DatabaseProvider.SaveCorrectionOfBalanceMaterials(document);
                NavigationManager.NavigateTo("/receipt-of-materials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
