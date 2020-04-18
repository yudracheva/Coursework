using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
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
    }
}
