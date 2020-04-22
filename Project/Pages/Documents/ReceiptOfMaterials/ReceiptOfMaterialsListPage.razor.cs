using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using System.Collections.Generic;

namespace Project.Pages.Documents.ReceiptOfMaterials
{
    public partial class ReceiptOfMaterialsListPage
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected List<ActOfReceipt> documents;

        protected override void OnInitialized()
        {
            UpdateData();
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/receipt-of-materials/{id:0}");
        }

        protected void Remove(int id)
        {
            DatabaseProvider.RemoveActOfReceipt(id);
            UpdateData();
        }

        private void UpdateData()
        {
            isLoad = false;

            documents = DatabaseProvider.GetActsOfReceipt();

            isLoad = true;

            StateHasChanged();
        }
    }
}
