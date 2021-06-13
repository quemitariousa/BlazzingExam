using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace HelperComponents
{
    public class SweetAlert
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public SweetAlert(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/HelperComponents/js/SweetAlert/SweetAlerts.js").AsTask());
        }

        public async ValueTask<bool> ShowConfirmDialog(string title, string text, string confirmText = "بله", string cancelText = "خیر")
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("ConfirmSweetAlert", title, text, confirmText, cancelText);
        }

        public async ValueTask ShowSuccessMessage(string title,string text,int timer = 2000)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("ShowSweetAlert", title, text, timer, "success");
        }

        public async ValueTask ShowErrorMessage(string title,string text,int timer = 2000)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("ShowSweetAlert", title, text, timer, "error");
        }

        public async ValueTask ToastSuccess(string title, string text, int timer = 3000)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("ToastSweetAlert", title, text, timer, "success");
        }

        public async ValueTask ToastError(string title, string text, int timer = 3000)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("ToastSweetAlert", title, text, timer, "error");
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}