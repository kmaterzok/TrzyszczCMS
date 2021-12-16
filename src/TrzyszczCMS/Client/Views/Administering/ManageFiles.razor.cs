using System.ComponentModel;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManageFiles
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
                await this.ViewModel.LoadFilesAsync();
            }
        }
        #endregion
    }
}
