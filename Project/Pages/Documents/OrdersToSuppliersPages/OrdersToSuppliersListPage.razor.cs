using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using System.Collections.Generic;

namespace Project.Pages.Documents.OrdersToSuppliersPages
{
    public partial class OrdersToSuppliersListPage
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected List<OrdersToSuppliers> documents;

        protected override void OnAfterRender(bool firstRender)
        {
            UpdateData();
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/orders-to-suppliers/{id:0}");
        }

        protected void Remove(int id)
        {
            DatabaseProvider.RemoveOrdersToSuppliers(id);
            UpdateData();
        }

        private void UpdateData()
        {
            isLoad = false;

            documents = DatabaseProvider.GetOrdersToSuppliers();

            isLoad = true;

            StateHasChanged();
        }
    }
}
