using Core.Application.Services.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Client.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using TrzyszczCMS.Client.Other;

namespace TrzyszczCMS.Client.Services.Implementations
{
    public class AuthService : IAuthService
    {
        #region Fields
        /// <summary>
        /// Reference on the service that manages tokens.
        /// </summary>
        private readonly ITokenService _tokenService;
        /// <summary>
        /// Used for verifying credentials and getting auth data.
        /// </summary>
        private readonly IRestAuthService _authRestService;
        /// <summary>
        /// Application's provider of authentication state. 
        /// </summary>
        private readonly ApplicationAuthenticationStateProvider _authStateProvider;
        #endregion

        #region Ctor
        public AuthService(ITokenService tokenService, IRestAuthService authRestService, AuthenticationStateProvider authStateProvider)
        {
            this._tokenService = tokenService;
            this._authRestService = authRestService;
            this._authStateProvider = (ApplicationAuthenticationStateProvider)authStateProvider;
        }
        #endregion

        #region Methods
        public async Task<bool> AuthenticateWithCredentialsAsync(string username, string password, bool remember)
        {
            var authUserInfo = await this._authRestService.GenerateAuthData(username, password, remember);
            if (null != authUserInfo)
            {
                await this._tokenService.SetTokenAsync(authUserInfo.AccessToken);
                await this._authStateProvider.NotifyAuthenticationStateChange();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task RevokeAuthenticationAsync()
        {
            await this._authRestService.RevokeToken(await this._tokenService.GetTokenAsync());
            await this._tokenService.RevokeTokenAsync();
            await this._authStateProvider.NotifyAuthenticationStateChange();
        }
        #endregion
    }
}
