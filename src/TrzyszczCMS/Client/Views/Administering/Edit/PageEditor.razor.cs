using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Views.Shared.Editors;

namespace TrzyszczCMS.Client.Views.Administering.Edit
{
    public partial class PageEditor : IDisposable
    {
        #region Init
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await this.ViewModel.LoadDataFromDeposit();
            }
        }
        #endregion

        #region Dispose
        public void Dispose() => this.ViewModel?.Dispose();
        #endregion

    }
}
