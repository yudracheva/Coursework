using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Pages.Documents.ReceiptOfMaterials
{
    public partial class ReceiptOfMaterialsPage
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected ActOfReceipt document;
        protected bool isLoad;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;
        protected List<Material> materials;
        protected string selectedDate;

        protected void ChangeDate(ChangeEventArgs changeEventArgs)
        {
            selectedDate = PageUtils.ChangeDate(changeEventArgs.Value);
        }

        protected void ChangeCount(ChangeEventArgs agrs, int numberLine)
        {
            PageUtils.ChangeCount(agrs.Value, document.Materials, numberLine);
        }

        protected void ChangePrice(ChangeEventArgs agrs, int numberLine)
        {
            PageUtils.ChangePrice(agrs.Value, document.Materials, numberLine);
        }

        protected void ChangeSum(ChangeEventArgs agrs, int numberLine)
        {
            PageUtils.ChangeSum(agrs.Value, document.Materials, numberLine);
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
                    document = DatabaseProvider.GetActOfReceipt(Id);
                    selectedSupplier = document.Supplier?.Id ?? 0;
                    selectedDate = document.CreatedDate.ToString(DATE_TO_PAGE_STRING_FORMAT);
                }
                else
                {
                    document = new ActOfReceipt();
                    selectedDate = DateTime.Now.ToString(DATE_TO_PAGE_STRING_FORMAT);
                }

                suppliers = DatabaseProvider.GetSuppliers();
                materials = DatabaseProvider.GetMaterials(selectedSupplier);

                isLoad = true;

                StateHasChanged();
            }
        }

        protected void Save()
        {
            try
            {
                if (selectedSupplier != 0)
                {
                    var category = suppliers.Where(d => d.Id.Equals(selectedSupplier)).FirstOrDefault();
                    document.Supplier = category;
                }
                else
                {
                    document.Supplier = null;
                }

                document.CreatedDate = DateTime.Parse(selectedDate);

                foreach (var item in document.Materials)
                {
                    item.Material = materials.FirstOrDefault(d => d.Id == item.SelectedMaterial);
                }

                DatabaseProvider.SaveActOfReceipt(document);
                NavigationManager.NavigateTo("/receipt-of-materials");
                ShowMessage($"Документ успешно сохранен", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить документ. {ex.Message}", Models.MessageType.Error);
            }
        }

    }
}
