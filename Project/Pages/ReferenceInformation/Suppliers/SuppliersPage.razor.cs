using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            isLoad = false;

            suppliers = DatabaseProvider.GetSuppliers();

            isLoad = true;
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/suppliers/{id:0}");
        }

        protected void Remove(int id)
        {
            // TODO: Добавить удаление
        }
    }
}
