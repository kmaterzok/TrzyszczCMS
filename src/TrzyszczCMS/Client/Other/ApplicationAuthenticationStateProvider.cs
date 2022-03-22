using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;
using TrzyszczCMS.Core.Shared.Models.Auth;
using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Core.Application.Services.Interfaces;
using TrzyszczCMS.Core.Shared.Helpers;

namespace TrzyszczCMS.Client.Other
{
    /// <summary>
    /// The provider of authorisation status for the application.
    /// </summary>
    public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region Fields
        /// <summary>
        /// Used for managing token for authorisation.
        /// </summary>
        private readonly ITokenService _tokenService;
        /// <summary>
        /// Used for verifying credentials and getting auth data.
        /// </summary>
        private readonly IRestAuthService _authDbService;
        #endregion

        #region Ctor
        /// <summary>
        /// Create an instance of <see cref="AuthenticationStateProvider"/> compliant object.
        /// </summary>
        public ApplicationAuthenticationStateProvider(ITokenService tokenService, IRestAuthService authDbService)
        {
            this._tokenService = tokenService;
            this._authDbService = authDbService;
        }
        #endregion

        #region Protected methods
        protected ClaimsIdentity GetClaimsIdentity(AuthUserInfo user)
        {
            var claimsIdentity = (user != null && user.Username != null) ?
                new ClaimsIdentity(ClaimsHelper.ResolveClaims(user), Constants.APPLICATION_AUTH_TYPE_NAME) :
                new ClaimsIdentity();
            
            return claimsIdentity;
        }
        #endregion

        #region Methods
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string accessToken = await _tokenService.GetTokenAsync();

            ClaimsIdentity identity;

            if (string.IsNullOrEmpty(accessToken))
            {
                identity = new ClaimsIdentity();
            }
            else
            {
                AuthUserInfo user = await this._authDbService.GetAuthData(accessToken);
                if (user == null)
                {
                    await this._tokenService.RevokeTokenAsync();
                }
                identity = GetClaimsIdentity(user);
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        /// <summary>
        /// Asynchronously notifies about change in the authentication state.
        /// </summary>
        /// <returns>A task for notification</returns>
        public async Task NotifyAuthenticationStateChange()
        {
            var authState = await this.GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        #endregion
    }
}
