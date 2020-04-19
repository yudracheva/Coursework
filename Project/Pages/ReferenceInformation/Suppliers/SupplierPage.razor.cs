using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

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

        protected List<Bank> banks;

        protected string oldName;

        protected bool isLoad;

        protected int selectedBank;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                supplier = DatabaseProvider.GetSupplier(Id);
                oldName = supplier?.OrganizationName;
                selectedBank = supplier.Bank?.Id ?? 0;
            }
            else
            {
                supplier = new Supplier();
            }

            banks = DatabaseProvider.GetBanks();

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
                if (selectedBank != 0)
                {
                    var bank = banks.Where(d => d.Id.Equals(selectedBank)).FirstOrDefault();
                    supplier.Bank = bank;
                }
                else
                {
                    supplier.Bank = null;
                }

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
