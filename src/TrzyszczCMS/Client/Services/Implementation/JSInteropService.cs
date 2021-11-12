using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model.JSInterop;
using TrzyszczCMS.Client.Services.Interfaces;

namespace TrzyszczCMS.Client.Services.Implementation
{
    public class JSInteropService : IJSInteropService
    {
        #region Fields
        private readonly IJSRuntime _jsRuntime;
        #endregion

        #region Ctor
        public JSInteropService(IJSRuntime jSRuntime)
        {
            this._jsRuntime = jSRuntime;
        }
        #endregion

        #region Methods
        public async Task<SelectionRange> GetSelectionRangeAsync(string componentId) =>
            await _jsRuntime.InvokeAsync<SelectionRange>("getSelectionRange", new[] { componentId });

        public async Task SelectTextIndexAsync(string componentId, int index, Action stateHasChanged) =>
            await SelectTextRangeAsync(componentId, index, index, stateHasChanged);

        public async Task SelectTextRangeAsync(string componentId, int start, int end, Action stateHasChanged)
        {
            stateHasChanged.Invoke();
            await _jsRuntime.InvokeAsync<object>("setSelectionRange", new object[] { componentId, start, end });
        }
        #endregion
    }
}
