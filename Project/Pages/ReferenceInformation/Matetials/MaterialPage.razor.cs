using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System;

namespace Project.Pages.ReferenceInformation.Matetials
{
    public class MaterialPageIndex : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected Material material;

        protected string oldName;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                material = DatabaseProvider.GetMaterial(Id);
                oldName = material?.Name;
            }
            else
            {
                material = new Material();
            }

            isLoad = true;
        }

        protected string GetDescription()
        {
            if (material.Id == 0)
                return "";

            return material.Name;
        }

        protected void Save()
        {
            try
            {
                DatabaseProvider.SaveMaterial(material);
                NavigationManager.NavigateTo("/materials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
