using Microsoft.AspNetCore.Components;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Reports.StatementOfUnpaidMaterials
{
    public partial class StatementOfUnpaidMaterialsPage
    {
        protected List<LineOfMaterials> lines;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            isLoad = false;
            try
            {
                lines = DatabaseProvider.GetNotPaymedActsOfReceipt();
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось загрузить данные для отчета. {ex.Message}", Models.MessageType.Error);
            }

            isLoad = true;

            StateHasChanged();
        }
    }
}
