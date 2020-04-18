using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models;
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
    }
}
