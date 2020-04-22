using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
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
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedDate = date.ToString("yyyy-MM-ddThh:mm");
        }

        protected void Dds()
        {

        }

        protected void ChangeCount(ChangeEventArgs agrs, int numberLine)
        {
            var line = document.Materials.FirstOrDefault(d => d.Number == numberLine);
            line.Count = Convert.ToInt32(agrs.Value);
            line.Sum = line.Price * line.Count;
        }

        protected void ChangePrice(ChangeEventArgs agrs, int numberLine)
        {
            var line = document.Materials.FirstOrDefault(d => d.Number == numberLine);
            line.Price = Convert.ToInt32(agrs.Value);
            line.Sum = line.Price * line.Count;
        }

        protected void ChangeSum(ChangeEventArgs agrs, int numberLine)
        {
            var line = document.Materials.FirstOrDefault(d => d.Number == numberLine);
            line.Sum = Convert.ToInt32(agrs.Value);
            if (line.Count != 0)
                line.Price = line.Sum / line.Count;
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

        protected override void OnInitialized()
        {
            isLoad = false;

            if (Id != 0)
            {
                document = DatabaseProvider.GetOrderToSupplier(Id);
                selectedSupplier = document.Supplier?.Id ?? 0;
                selectedDate = document.CreatedDate.ToString("yyyy-MM-ddThh:mm");
            }
            else
            {
                document = new OrdersToSuppliers();
                selectedDate = DateTime.Now.ToString("yyyy-MM-ddThh:mm");
            }

            suppliers = DatabaseProvider.GetSuppliers();
            materials = DatabaseProvider.GetMaterials();

            isLoad = true;

            StateHasChanged();
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

                DatabaseProvider.SaveOrderToSupplier(document);
                NavigationManager.NavigateTo("/receipt-of-materials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить. {ex.Message}");
            }
        }
    }
}
