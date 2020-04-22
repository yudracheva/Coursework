using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using System.Collections.Generic;

namespace Project.Pages.Documents.AdjustmentOfTheBalanceOfMaterials
{
    public partial class AdjustmentOfTheBalanceOfMaterialsListPage
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected List<CorrectionOfBalanceMaterials> documents;

        protected override void OnInitialized()
        {
            UpdateData();
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/adjustment-of-the-balance-of-materials/{id:0}");
        }

        protected void Remove(int id)
        {
            DatabaseProvider.RemoveCorrectionOfBalanceMaterials(id);
            UpdateData();
        }

        private void UpdateData()
        {
            isLoad = false;

            documents = DatabaseProvider.GetCorrectionsOfBalanceMaterials();

            isLoad = true;

            StateHasChanged();
        }
    }
}
