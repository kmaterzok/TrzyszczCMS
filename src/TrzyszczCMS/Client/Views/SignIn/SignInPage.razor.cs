using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;

namespace TrzyszczCMS.Client.Views.SignIn
{
    public partial class SignInPage
    {
        #region Init
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ClaimsPrincipal claimsPrincipal = (await AuthStateProvider.GetAuthenticationStateAsync()).User;

                if (claimsPrincipal.Identity.IsAuthenticated)
                {
                    NavigationManager.NavigateTo(Constants.AFTER_SIGN_IN_IMMEDIATE_PAGE_URL);
                }
                // TODO: Add spinner indicating the process of signing in.
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        #endregion

        #region Methods
        private async Task SignInAsync() => await ViewModel.SignInUser(() =>
        {
            NavigationManager.NavigateTo(Constants.AFTER_SIGN_IN_IMMEDIATE_PAGE_URL);
            this.StateHasChanged();
        });
        #endregion
    }
}
