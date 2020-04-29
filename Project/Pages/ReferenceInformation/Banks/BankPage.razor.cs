using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System;

namespace Project.Pages.ReferenceInformation.MaterialCategories
{
    public class BankPageIndex : DefaultComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        protected Bank bank;

        protected string oldName;

        protected bool isLoad;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                isLoad = false;

                if (Id != 0)
                {
                    bank = DatabaseProvider.GetBank(Id);
                    oldName = bank?.Name;
                }
                else
                {
                    bank = new Bank();
                }

                isLoad = true;

                StateHasChanged();
            }
        }

        protected string GetDescription()
        {
            if (bank.Id == 0)
                return "";

            return bank.Name;
        }

        protected void Save()
        {
            try
            {
                DatabaseProvider.SaveBank(bank);
                NavigationManager.NavigateTo("/banks");
                ShowMessage($"Банк успешно сохранен", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить. {ex.Message}", Models.MessageType.Error);
            }
        }
    }
}
