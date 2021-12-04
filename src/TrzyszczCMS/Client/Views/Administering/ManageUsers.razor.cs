using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManageUsers
    {
        #region Init
        protected override void OnInitialized()
        {
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );
            base.OnInitialized();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await this.ViewModel.LoadUsersWithFilter();
            }
        }
        #endregion

        #region Methods
        private async Task GoToManagingUserAsync(int userId)
        {
            await this.ViewModel.SendDataToDepositoryForEditingAsync(userId);
            this.NavigationManager.NavigateTo("/manage/edit-user");
        }

        private async Task GoToCreatingUserAsync()
        {
            await this.ViewModel.SendDataToDepositoryForCreatingAsync();
            this.NavigationManager.NavigateTo("/manage/edit-user");
        }
        #endregion
    }
}
