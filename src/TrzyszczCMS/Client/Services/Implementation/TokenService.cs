using Blazored.LocalStorage;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;
using Core.Application.Services.Interfaces;

namespace TrzyszczCMS.Client.Services.Implementations
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
            if (await _localStorage.ContainKeyAsync(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME))
            {
                return await _localStorage.GetItemAsStringAsync(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME);
            }
            return null;
        }

        public async Task RevokeTokenAsync()
        {
            if (await _localStorage.ContainKeyAsync(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME))
            {
                await _localStorage.RemoveItemAsync(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME);
            }
        }

        public async Task SetTokenAsync(string accessToken)
        {
            await this._localStorage.SetItemAsStringAsync(Constants.LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME, accessToken);
        }
    }
}
