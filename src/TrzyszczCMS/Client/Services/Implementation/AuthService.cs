using Core.Application.Services.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Client.Services.Interfaces;
using System.Threading.Tasks;

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
        #endregion

        #region Ctor
        public AuthService(ITokenService tokenService, IRestAuthService authRestService)
        {
            this._tokenService = tokenService;
            this._authRestService = authRestService;
        }
        #endregion

        #region Methods
        public async Task<bool> AuthenticateWithCredentialsAsync(string username, string password, bool remember)
        {
            var authUserInfo = await this._authRestService.GenerateAuthData(username, password, remember);
            if (null != authUserInfo)
            {
                await this._tokenService.SetTokenAsync(authUserInfo.AccessToken);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task RevokeAuthenticationAsync()
        {
            await this._tokenService.RevokeTokenAsync();
        }
        #endregion
    }
}
