using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Pages.ReferenceInformation.MaterialCategories
{
    public class MaterialCategoryPageIndex : ComponentBase
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<MaterialСategory> materialСategories;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            materialСategories = DatabaseProvider.GetMaterialСategories();

            isLoad = true;
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/material-categories/{id:0}");
        }

        protected void Remove(int id)
        {
            // TODO: Добавить удаление
        }
    }
}
