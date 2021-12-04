using Core.Application.Helpers.Extensions;
using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Models.ManageUser;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class ManageUserService : IManageUserService
    {
        #region Fields
        private readonly HttpClient _authHttpClient;
        #endregion

        #region Ctor
        public ManageUserService(IHttpClientFactory httpClientFactory)
        {
            this._authHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_AUTH_NAME);
        }
        #endregion

        #region Methods
        public async Task<List<SimpleUserInfo>> GetSimpleUserInfo([NotNull] Dictionary<FilteredGridField, string> filters)
        {
            var response = await this._authHttpClient.PostAsJsonAsync(
                "/ManageUser/SimpleUserInfo", filters
            );
            return await response.ContentOrDefaultAsync<List<SimpleUserInfo>>();
        }

        public async Task DeleteUser(int userId) =>
            (await this._authHttpClient.DeleteAsync($"/ManageUser/DeleteUser/{userId}")).EnsureSuccessStatusCode();

        public async Task<DetailedUserInfo> GetDetailedUserInfo(int userId) =>
            await this._authHttpClient.GetFromJsonAsync<DetailedUserInfo>($"/ManageUser/DetailedUserInfo/{userId}");

        public async Task<List<SimpleRoleInfo>> GetSimpleRoleInfo() =>
            await this._authHttpClient.GetFromJsonAsync<List<SimpleRoleInfo>>($"/ManageUser/SimpleRoleInfo");
        #endregion
    }
}
