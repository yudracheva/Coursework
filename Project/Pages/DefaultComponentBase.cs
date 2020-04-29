using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Project.Interfaces;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages
{
    public class DefaultComponentBase : ComponentBase
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected void ShowMessage(string message, MessageType type)
        {
            var jsFunc = GetFuncForShowMessage(type);

            var task = Task.Run(() => JsRuntime.InvokeAsync<string>(jsFunc, message));
            
            task.Wait();

            Console.WriteLine(message);
        }

        private string GetFuncForShowMessage(MessageType serviceDocumentType) => serviceDocumentType switch
        {
            MessageType.Info => "workWithMessage.showInfo",
            MessageType.Error => "workWithMessage.showError",
            MessageType.Warning => "workWithMessage.showWarning",
            MessageType.Success => "workWithMessage.showSuccess",
            _ => "workWithMessage.showInfo",
        };
    }
}
