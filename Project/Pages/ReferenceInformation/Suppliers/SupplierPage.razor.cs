using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
using Project.Models.ReferenceInformation;
using System;

namespace Project.Pages.ReferenceInformation.Suppliers
{
    public class SupplierPageIndex : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected Supplier supplier;

        protected string oldName;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                supplier = DatabaseProvider.GetSupplier(Id);
                oldName = supplier?.OrganizationName;
            }
            else
            {
                supplier = new Supplier();
            }

            isLoad = true;
        }

        protected string GetDescription()
        {
            if (supplier.Id == 0)
                return "";

            return supplier.OrganizationName;
        }

        protected void Save()
        {
            try
            {
                DatabaseProvider.SaveSupplier(supplier);
                NavigationManager.NavigateTo("/suppliers");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
