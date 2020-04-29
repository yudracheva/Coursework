using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.AdjustmentOfTheBalanceOfMaterials
{
    public partial class AdjustmentOfTheBalanceOfMaterialsPage
    {
        [Parameter]
        public int Id { get; set; }

        protected bool isLoad;

        protected CorrectionOfBalanceMaterials document;
        protected List<Material> materials;
        protected string selectedDate;

        protected void ChangeDate(ChangeEventArgs changeEventArgs)
        {
            selectedDate = PageUtils.ChangeDate(changeEventArgs.Value);
        }

        protected void ChangeCount(ChangeEventArgs agrs, int numberLine)
        {
            PageUtils.ChangeCount(agrs.Value, document.Materials, numberLine, false);
        }

        protected void AddLine()
        {
            document.Materials.Add(new LineOfMaterials());
            ChangesNumbers();
            StateHasChanged();
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
            if (firstRender)
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
        }

        protected void Save()
        {
            try
            {
                document.CreatedDate = DateTime.Parse(selectedDate);

                foreach (var item in document.Materials)
                {
                    item.Material = materials.FirstOrDefault(d => d.Id == item.SelectedMaterial);

                    if (item.Count == 0)
                    {
                        ShowMessage($"Невозможно сохранить документ, т.к. в строке {item.Number} не установлено количество.", Models.MessageType.Error);
                        return;
                    }
                }

                DatabaseProvider.SaveCorrectionOfBalanceMaterials(document);
                NavigationManager.NavigateTo("/adjustment-of-the-balance-of-materials");

                ShowMessage($"Документ успешно сохранен", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить документ. {ex.Message}", Models.MessageType.Error);
            }
        }

        protected void Remove(int number)
        {
            PageUtils.RemoveLine(document.Materials, number);
        }

        protected void Remove()
        {
            try
            {
                DatabaseProvider.RemoveCorrectionOfBalanceMaterials(Id);
                NavigationManager.NavigateTo("/adjustment-of-the-balance-of-materials");
                ShowMessage($"Документ успешно удален", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить документ. {ex.Message}", Models.MessageType.Error);
            }
        }
    }
}
