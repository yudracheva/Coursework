using Microsoft.AspNetCore.Components;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.ReceiptOfMaterials
{
    public partial class ReceiptOfMaterialsPage
    {
        [Parameter]
        public int Id { get; set; }

        protected ActOfReceipt document;
        protected bool isLoad;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;
        protected List<Material> materials;
        protected string selectedDate;
        protected int selectedOrder;
        protected List<OrdersToSuppliers> orders;

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
                    selectedOrder = document?.Order?.Number ?? 0;
                }
                else
                {
                    document = new ActOfReceipt();
                    selectedDate = DateTime.Now.ToString(DATE_TO_PAGE_STRING_FORMAT);
                }

                suppliers = DatabaseProvider.GetSuppliers();
                materials = DatabaseProvider.GetMaterials(selectedSupplier);
                orders = DatabaseProvider.GetAvailableOrders();

                isLoad = true;

                StateHasChanged();
            }
        }

        protected void Remove(int number)
        {
            PageUtils.RemoveLine(document.Materials, number);
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
                    ShowMessage("Выберите поставщика перед записью", Models.MessageType.Error);
                    return;
                }

                if (selectedOrder != 0)
                {
                    var order = orders.Where(d => d.Number.Equals(selectedOrder)).FirstOrDefault();
                    document.Order = order;
                }
                else
                {
                    document.Order = null;
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

                    if (item.Price == 0)
                    {
                        ShowMessage($"Невозможно сохранить документ, т.к. в строке {item.Number} не установлена цена.", Models.MessageType.Error);
                        return;
                    }
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

        protected void Remove()
        {
            try
            {
                DatabaseProvider.RemoveActOfReceipt(Id);
                NavigationManager.NavigateTo("/receipt-of-materials");
                ShowMessage($"Документ успешно удален", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить документ. {ex.Message}", Models.MessageType.Error);
            }
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
                    selectedOrder = 0;
                }
                selectedSupplier = parsSupplier;
                materials = DatabaseProvider.GetMaterials(selectedSupplier);
                orders = DatabaseProvider.GetAvailableOrders();
                StateHasChanged();
            }
        }

        protected void ChangeSelectedOrder(ChangeEventArgs args)
        {
            if (String.IsNullOrEmpty(args.Value?.ToString()))
            {
                selectedOrder = 0;
            }
            else
            {
                var parsNUmberOrder = Convert.ToInt32(args.Value);
                selectedOrder = parsNUmberOrder;
                
                var order = orders.FirstOrDefault(d => d.Number == selectedOrder);
                if (order != null)
                {
                    selectedSupplier = order.Supplier.Id;
                    var availableMaterialFromOrder = DatabaseProvider.GetAvailableOrderMaterial(order.Number);
                    document.Materials = availableMaterialFromOrder;
                }

                orders = DatabaseProvider.GetAvailableOrders();
                materials = DatabaseProvider.GetMaterials(selectedSupplier);

                StateHasChanged();
            }
        }
    }
}
