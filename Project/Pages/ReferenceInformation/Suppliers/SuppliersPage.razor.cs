using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Pages.ReferenceInformation.Suppliers
{
    public class SuppliersPageIndex : ComponentBase
    {

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<Supplier> suppliers;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            Update();
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/suppliers/{id:0}");
        }

        protected void Remove(int id)
        {
            DatabaseProvider.RemoveSupplier(id);
            Update();
        }

        protected void Update()
        {
            isLoad = false;

            suppliers = DatabaseProvider.GetSuppliers();

            isLoad = true;
        }
    }
}
