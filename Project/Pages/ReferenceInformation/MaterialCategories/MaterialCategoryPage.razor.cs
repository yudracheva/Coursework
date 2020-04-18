using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System;

namespace Project.Pages.ReferenceInformation.MaterialCategories
{
    public class MaterialCategoriesPageIndex : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected MaterialСategory materialСategory;

        protected string oldName;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                materialСategory = DatabaseProvider.GetMaterialСategory(Id);
                oldName = materialСategory?.Name;
            }
            else
            {
                materialСategory = new MaterialСategory();
            }

            isLoad = true;
        }

        protected string GetDescription()
        {
            if (materialСategory.Id == 0)
                return "";

            return materialСategory.Name;
        }

        protected void Save()
        {
            Console.WriteLine("Save");
            NavigationManager.NavigateTo("/suppliers");
        }
    }
}
