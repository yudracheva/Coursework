using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

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
        protected List<MaterialCategory> materialCategories;
        protected List<Supplier> suppliers;
        protected int selectedMaterialCategory;
        protected int selectedSupplier;
        protected string oldName;
        protected bool isLoad;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                isLoad = false;

                if (Id != 0)
                {
                    material = DatabaseProvider.GetMaterial(Id);
                    oldName = material?.Name;
                    selectedMaterialCategory = material.Category?.Id ?? 0;
                    selectedSupplier = material.Supplier?.Id ?? 0;
                }
                else
                {
                    material = new Material();
                }

                materialCategories = DatabaseProvider.GetMaterialСategories();
                suppliers = DatabaseProvider.GetSuppliers();

                isLoad = true;

                StateHasChanged();
            }
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
                if (selectedMaterialCategory != 0)
                {
                    var category = materialCategories.Where(d => d.Id.Equals(selectedMaterialCategory)).FirstOrDefault();
                    material.Category = category;
                }
                else
                {
                    material.Category = null;
                }

                if (selectedSupplier != 0)
                {
                    var supplier = suppliers.Where(d => d.Id.Equals(selectedSupplier)).FirstOrDefault();
                    material.Supplier = supplier;
                }
                else
                {
                    material.Supplier = null;
                }

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
