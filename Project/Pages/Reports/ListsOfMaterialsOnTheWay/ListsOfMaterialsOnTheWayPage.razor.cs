using Microsoft.AspNetCore.Components;
using Project.Models.ReferenceInformation;
using Project.Models.Reports;
using Project.Pages.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.Reports.ListsOfMaterialsOnTheWay
{
    public partial class ListsOfMaterialsOnTheWayPage
    {

        [Parameter]
        public int MaterialId { get; set; }

        protected List<ReportListsOfMaterialsOnTheWay> lines;
        protected string selectedBeginDate;
        protected string selectedEndDate;
        protected int selectedMaterial;
        protected List<Material> materials;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                materials = DatabaseProvider.GetMaterials();
                selectedEndDate = DateTime.Today.ToString(DATE_TO_PAGE_STRING_FORMAT);
                selectedBeginDate = DateTime.Today.AddMonths(-1).ToString(DATE_TO_PAGE_STRING_FORMAT);

                UpdateData();
            }
        }

        private void UpdateData()
        {
            isLoad = false;
            try
            {
                lines = DatabaseProvider.GetReportListsOfMaterialsOnTheWay();

                var beginDate = DateTime.Parse(selectedBeginDate);
                var endDate = DateTime.Parse(selectedEndDate);

                if (selectedMaterial != 0)
                    lines = lines.Where(d => d.Material?.Id == selectedMaterial).ToList();

                lines = lines.Where(d => d.DocumentDate > beginDate && d.DocumentDate < endDate).ToList();
                if (MaterialId != 0)
                {
                    lines = lines.Where(d => (d.Material != null) && (d.Material.Id.Equals(MaterialId))).ToList();
                    selectedMaterial = MaterialId;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Не удалось загрузить данные для отчета", Models.MessageType.Error);
            }

            isLoad = true;

            StateHasChanged();
        }

        protected void ChangeEndDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedEndDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);

            UpdateData();
        }

        protected void ChangeBeginDate(ChangeEventArgs changeEventArgs)
        {
            var date = DateTime.Parse(changeEventArgs.Value.ToString());
            selectedBeginDate = date.ToString(DATE_TO_PAGE_STRING_FORMAT);

            UpdateData();
        }

        protected void ChangeSelectedMaterial(ChangeEventArgs changeEventArgs)
        {
            selectedMaterial = Convert.ToInt32(changeEventArgs.Value);

            UpdateData();
        }
    }
}
