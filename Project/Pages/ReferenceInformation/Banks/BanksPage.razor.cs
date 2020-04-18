using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Pages.ReferenceInformation.MaterialCategories
{
    public class BanksPageIndex : ComponentBase
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<Bank> banks;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            banks = DatabaseProvider.GetBanks();

            isLoad = true;
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/banks/{id.ToString("0")}");
        }

        protected void Remove(int id)
        {
            // TODO: Добавить удаление
        }
    }
}
