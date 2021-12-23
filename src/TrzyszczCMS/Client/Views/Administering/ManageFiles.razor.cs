using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManageFiles
    {
        #region Properties
        [CascadingParameter]
        public Popupper Popupper { get; private set; }
        #endregion

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

        #region Methods
        private void CreateDirectory()
        {
            Popupper.ShowPrompt("Enter the name for a new directory.", async answer =>
            {
                if (answer.GetValue(out string name, out _))
                {
                    if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                    {
                        Popupper.ShowAlert("A name of the directory cannot be empty.");
                    }
                    else if (!await ViewModel.CreateDirectoryForCurrentNodeAsync(name))
                    {
                        Popupper.ShowAlert("The directory with the specified name has been existed over there.");
                    }
                }
            }, true);
        }
        #endregion
    }
}
