using Core.Shared.Models.ManageUser;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManageUsers
    {
        #region Constants
        /// <summary>
        /// The constant defining paddings within a body of an accordion.
        /// </summary>
        private const string ACCORDION_CONTENT_CLASS = "p-0 border-bottom border-dark border-1";
        #endregion

        #region Properties
        [CascadingParameter]
        private Popupper Popupper { get; set; }
        #endregion

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
                await this.ViewModel.LoadSignedInUserTokens();
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

        private void DeleteUser(SimpleUserInfo user)
        {
            this.Popupper.ShowYesNoPrompt($"Delete <em>{user.UserName}</em>?", async result =>
            {
                if (result == PopupExitResult.Yes)
                {
                    await ViewModel.DeleteUserAsync(user.Id);
                }
            });
        }
        #endregion
    }
}
