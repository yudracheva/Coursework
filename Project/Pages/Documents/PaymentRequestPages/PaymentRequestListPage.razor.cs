using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using System.Collections.Generic;

namespace Project.Pages.Documents.PaymentRequestPages
{
    public partial class PaymentRequestListPage
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected List<PaymentRequest> documents;

        protected override void OnInitialized()
        {
            UpdateData();
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/payment-requests/{id:0}");
        }

        protected void Remove(int id)
        {
            DatabaseProvider.RemovePaymentRequest(id);
            UpdateData();
        }

        private void UpdateData()
        {
            isLoad = false;

            documents = DatabaseProvider.GetPaymentsRequests();

            isLoad = true;

            StateHasChanged();
        }
    }
}
