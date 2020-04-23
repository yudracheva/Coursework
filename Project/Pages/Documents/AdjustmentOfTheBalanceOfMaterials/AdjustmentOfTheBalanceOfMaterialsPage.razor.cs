using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.AdjustmentOfTheBalanceOfMaterials
{
    public partial class AdjustmentOfTheBalanceOfMaterialsPage
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected bool isLoad;

        protected CorrectionOfBalanceMaterials document;
        protected List<Material> materials;
        protected string selectedDate;

        protected void ChangeDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);
        }

        protected void Dds()
        {

        }

        protected void ChangeCount(ChangeEventArgs agrs, int numberLine)
        {
            var line = document.Materials.FirstOrDefault(d => d.Number == numberLine);
            var count = Convert.ToInt32(agrs.Value);
            line.Count = count < 0 ? 0 : count;
        }

        protected void AddLine()
        {
            document.Materials.Add(new LineOfMaterials());
            ChangesNumbers();
        }

        private void ChangesNumbers()
        {
            for (int i = 0; i < document.Materials.Count; i++)
            {
                document.Materials[i].Number = i + 1;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            isLoad = false;

            if (Id != 0)
            {
                document = DatabaseProvider.GetCorrectionOfBalanceMaterials(Id);
                selectedDate = document.CreatedDate.ToString(DATE_TO_PAGE_STRING_FORMAT);
            }
            else
            {
                document = new CorrectionOfBalanceMaterials();
                selectedDate = DateTime.Now.ToString(DATE_TO_PAGE_STRING_FORMAT);
            }

            materials = DatabaseProvider.GetMaterials();

            isLoad = true;

            StateHasChanged();
        }

        protected void Save()
        {
            try
            {
                document.CreatedDate = DateTime.Parse(selectedDate);

                foreach (var item in document.Materials)
                {
                    item.Material = materials.FirstOrDefault(d => d.Id == item.SelectedMaterial);
                }

                DatabaseProvider.SaveCorrectionOfBalanceMaterials(document);
                NavigationManager.NavigateTo("/adjustment-of-the-balance-of-materials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
