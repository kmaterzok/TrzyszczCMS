using ApplicationCore.Services.Interfaces.DbAccess;
using System.Threading.Tasks;
using TrzyszczCMS.Services.Interfaces;

namespace TrzyszczCMS.Services.Implementations
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
        private readonly IAuthDatabaseService _authDbService;
        #endregion

        #region Ctor
        public AuthService(ITokenService tokenService, IAuthDatabaseService authDbService)
        {
            this._tokenService = tokenService;
            this._authDbService = authDbService;
        }
        #endregion

        #region Methods
        public async Task<bool> AuthenticateWithCredentialsAsync(string username, string password, bool remember)
        {
            var authUserInfo = await this._authDbService.GenerateAuthData(username, password, remember);
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
