using Blazored.LocalStorage;
using System.Threading.Tasks;
using TrzyszczCMS.Data;
using TrzyszczCMS.Services.Interfaces;

namespace TrzyszczCMS.Services.Implementations
{
    public class TokenService : ITokenService
    {
        #region Fields
        private readonly ILocalStorageService _localStorage;
        #endregion

        #region Ctor
        public TokenService(ILocalStorageService localStorage)
        {
            this._localStorage = localStorage;
        }
        #endregion


        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME);
        }

        public async Task RevokeTokenAsync()
        {
            await _localStorage.RemoveItemAsync("accessToken");

            // TODO: Remove from database.
        }

        public async Task SetTokenAsync(string accessToken)
        {
            await this._localStorage.SetItemAsync<string>(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME, accessToken);
        }
    }
}
