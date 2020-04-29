using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages.Documents.PaymentRequestPages
{
    public partial class PaymentRequestPage
    {
        [Parameter]
        public int Id { get; set; }

        protected PaymentRequest document;
        protected bool isLoad;
        protected int selectedSupplier;
        protected List<Supplier> suppliers;
        protected List<ActOfReceipt> acts;
        protected string selectedDate;
        protected int selectedAct;

        protected void ChangeDate(ChangeEventArgs changeEventArgs)
        {
            selectedDate = PageUtils.ChangeDate(changeEventArgs.Value);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                isLoad = false;

                if (Id != 0)
                {
                    document = DatabaseProvider.GetPaymentRequest(Id);
                    selectedSupplier = document.Supplier?.Id ?? 0;
                    selectedDate = document.CreatedDate.ToString(DATE_TO_PAGE_STRING_FORMAT);
                    selectedAct = document.Act?.Number ?? 0;
                }
                else
                {
                    document = new PaymentRequest();
                    selectedDate = DateTime.Now.ToString(DATE_TO_PAGE_STRING_FORMAT);
                }

                suppliers = DatabaseProvider.GetSuppliers();
                acts = DatabaseProvider.GetActsOfReceipt();
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
                    ShowMessage("Сохранение невозможно. Укажите получателя.", Models.MessageType.Error);
                    return;
                }

                if (selectedAct != 0)
                {
                    var act = acts.Where(d => d.Number.Equals(selectedAct)).FirstOrDefault();
                    document.Act = act;
                }
                else
                {
                    ShowMessage("Сохранение невозможно. Укажите акт, на основание которого производится оплата.", Models.MessageType.Error);
                    return;
                }

                document.CreatedDate = DateTime.Parse(selectedDate);

                DatabaseProvider.SavePaymentRequest(document);
                NavigationManager.NavigateTo("/payment-requests");
                ShowMessage($"Документ успешно сохранен", Models.MessageType.Success);
            }
            catch (Exception ex)
            {
                ShowMessage($"Не удалось сохранить документ. {ex.Message}", Models.MessageType.Error);
            }
        }

        protected void ChangeSum(ChangeEventArgs args)
        {
            if (!String.IsNullOrEmpty(args?.Value?.ToString()))
            {
                var sum = Convert.ToDecimal(args.Value.ToString());
                if (sum <= 0)
                {
                    document.Sum = 0;
                }
                else
                {
                    document.Sum = sum < 0 ? 0 : sum;
                }
            }
            else
            {
                document.Sum = 0;
            }
        }

        protected void Remove()
        {
            try
            {
                DatabaseProvider.RemoveOrdersToSuppliers(Id);
                NavigationManager.NavigateTo("/payment-requests");
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
                selectedSupplier = parsSupplier;
                selectedAct = 0;

                StateHasChanged();
            }
        }

        protected void ChangeSelectedAct(ChangeEventArgs args)
        {
            if (String.IsNullOrEmpty(args.Value?.ToString()))
            {
                selectedAct = 0;
            }
            else
            {
                var parsAct = Convert.ToInt32(args.Value);
                selectedAct = parsAct;

                var act = DatabaseProvider.GetActOfReceipt(selectedAct);
                if (act != null)
                {
                    selectedSupplier = act.Supplier.Id;
                    document.Sum = act.Materials.Sum(d => d.Sum);
                }

                StateHasChanged();
            }
        }
    }
}
