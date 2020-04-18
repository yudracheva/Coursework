using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
using System.Collections.Generic;

namespace Project.Pages.ReferenceInformation.Matetials
{
    public class MaterialsPageIndex : ComponentBase
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<Material> materials;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            materials = DatabaseProvider.GetMaterials();

            isLoad = true;
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/materials/{id:0}");
        }

        protected void Remove(int id)
        {
            // TODO: Добавить удаление
        }
    }
}
