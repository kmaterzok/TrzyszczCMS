using Core.Application.Helpers.Extensions;
using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Models.Auth;
using Core.Shared.Models.Rest.Requests.Auth;
using Core.Shared.Models.Rest.Responses.Auth;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class RestAuthService : IRestAuthService
    {
        #region Fields
        private readonly HttpClient _noauthHttpClient;
        private readonly HttpClient _authHttpClient;
        #endregion

        #region Ctor
        public RestAuthService(IHttpClientFactory httpClientFactory)
        {
            this._noauthHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_ANON_NAME);
            this._authHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_AUTH_NAME);
        }
        #endregion

        #region Methods
        public async Task<AuthUserInfo> GetAuthData(string accessToken)
        {
            var response = await this._noauthHttpClient.GetAsync($"Auth/GetData/{accessToken}");
            return (await response.ContentOrDefaultAsync<GenerateAuthDataResponse>())?.AuthUserInfo;
        }
        
        public async Task<AuthUserInfo> GenerateAuthData(string username, string password, bool remember)
        {
            var response = await this._noauthHttpClient.PostAsJsonAsync("Auth/GenerateData", new GenerateAuthDataRequest()
            {
                Password = password,
                RememberMe = remember,
                Username = username
            });

            return (await response.ContentOrDefaultAsync<GenerateAuthDataResponse>())?.AuthUserInfo;
        }

        public async Task RevokeToken(string accessToken)
        {
            await this._authHttpClient.DeleteAsync($"Auth/RevokeAccessToken");
        }
        #endregion
    }
}
