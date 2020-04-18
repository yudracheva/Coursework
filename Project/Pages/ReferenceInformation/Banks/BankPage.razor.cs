using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
using Project.Models.ReferenceInformation;
using System;

namespace Project.Pages.ReferenceInformation.MaterialCategories
{
    public class BankPageIndex : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected Bank bank;

        protected string oldName;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                bank = DatabaseProvider.GetBank(Id);
                oldName = bank?.Name;
            }
            else
            {
                bank = new Bank();
            }

            isLoad = true;
        }

        protected string GetDescription()
        {
            if (bank.Id == 0)
                return "";

            return bank.Name;
        }

        protected void Save()
        {
            Console.WriteLine("Save");
            NavigationManager.NavigateTo("/banks");
        }
    }
}
