using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System;

namespace Project.Pages.ReferenceInformation.MaterialCategories
{
    public class MaterialCategoriesPageIndex : DefaultComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected MaterialCategory materialCategory;

        protected string oldName;

        protected bool isLoad;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                isLoad = false;

                if (Id != 0)
                {
                    materialCategory = DatabaseProvider.GetMaterialCategory(Id);
                    oldName = materialCategory?.Name;
                }
                else
                {
                    materialCategory = new MaterialCategory();
                }

                isLoad = true;

                StateHasChanged();
            }
        }

        protected string GetDescription()
        {
            if (materialCategory.Id == 0)
                return "";

            return materialCategory.Name;
        }

        protected void Save()
        {
            try
            {
                DatabaseProvider.SaveMaterialCategory(materialCategory);
                NavigationManager.NavigateTo("/material-categories");
                ShowMessage($"Категория успешно сохранена", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить. {ex.Message}", Models.MessageType.Error);
            }
        }
    }
}
