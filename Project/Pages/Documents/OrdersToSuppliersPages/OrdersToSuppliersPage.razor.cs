using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.OrdersToSuppliersPages
{
    public partial class OrdersToSuppliersPage
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected OrdersToSuppliers document;
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
                    document = DatabaseProvider.GetOrderToSupplier(Id);
                    selectedSupplier = document.Supplier?.Id ?? 0;
                    selectedDate = document.CreatedDate.ToString(DATE_TO_PAGE_STRING_FORMAT);
                }
                else
                {
                    document = new OrdersToSuppliers();
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
                    var supplier = suppliers.Where(d => d.Id.Equals(selectedSupplier)).FirstOrDefault();
                    document.Supplier = supplier;
                }
                else
                {
                    document.Supplier = null;
                }

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

                DatabaseProvider.SaveOrderToSupplier(document);
                NavigationManager.NavigateTo("/orders-to-suppliers");
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

        protected void ChangeSelectedSupplier(ChangeEventArgs args)
        {
            if (String.IsNullOrEmpty(args.Value?.ToString()))
            {
                selectedSupplier = 0;
            }
            else
            {
                var parsSupplier = Convert.ToInt32(args.Value);
                if (selectedSupplier != parsSupplier && document.Materials.Count > 0)
                {
                    ShowMessage("Список товаров очищен, т.к. как в списке присутствует товар не относящийся к текущему поставщику.", Models.MessageType.Info);
                    document.Materials = new List<LineOfMaterials>();
                }
                selectedSupplier = parsSupplier;
                materials = DatabaseProvider.GetMaterials(selectedSupplier);
                StateHasChanged();
            }
        }
    }
}
