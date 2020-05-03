using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using System.Collections.Generic;

namespace Project.Pages.Documents.Adjustment
{
    public partial class AdjustmentOfTheBalanceListPage
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected List<CorrectionOfBalanceMaterials> documents;

        protected override void OnAfterRender(bool firstRender)
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
