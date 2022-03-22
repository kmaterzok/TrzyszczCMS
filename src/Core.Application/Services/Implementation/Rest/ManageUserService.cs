using TrzyszczCMS.Core.Application.Helpers.Extensions;
using TrzyszczCMS.Core.Application.Models;
using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageUser;
using TrzyszczCMS.Core.Shared.Models.Rest.Requests.ManageUsers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Application.Services.Implementation.Rest
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

        public async Task<Result<Tuple<bool>, string>> UserNameExists(string username)
        {
            if (!RegexHelper.IsValidUserName(username))
            {
                // TODO: Move _PatternMismatch_ to constants.
                return Result<Tuple<bool>, string>.MakeError("PatternMismatch");
            }
            var checkResponse = await this._authHttpClient.GetAsync($"/ManageUser/UserNameExists/{username}"); ;
            return Result<Tuple<bool>, string>.MakeSuccess(new Tuple<bool>(checkResponse.IsSuccessStatusCode));
        }

        public async Task<string> AddUser(DetailedUserInfo user) =>
            await (await this._authHttpClient.PostAsJsonAsync($"/ManageUser/AddUser", user)).ContentOrFailAsync<string>();

        public async Task UpdateUser(DetailedUserInfo user) =>
            (await this._authHttpClient.PostAsJsonAsync($"/ManageUser/UpdateUser", user)).EnsureSuccessStatusCode();

        public async Task RevokeToken(int tokenId) =>
            (await this._authHttpClient.DeleteAsync($"/ManageUser/RevokeToken/{tokenId}")).EnsureSuccessStatusCode();

        public async Task<List<SimpleTokenInfo>> GetOwnSimpleTokenInfo() =>
            await this._authHttpClient.GetFromJsonAsync<List<SimpleTokenInfo>>($"/ManageUser/OwnSimpleTokenInfo");

        public async Task<PasswordNotChangedReason?> ChangeSignedInUserPassword(string currentPassword, string newPassword)
        {
            var request = new ChangeOwnPasswordRequest()
            {
                CurrentPassword = currentPassword,
                NewPassword     = newPassword
            };
            var response = await this._authHttpClient.PostAsJsonAsync($"/ManageUser/ChangeOwnPassword", request);
            if (System.Net.HttpStatusCode.Conflict == response.StatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PasswordNotChangedReason>();
            }
            return null;
        }
        #endregion
    }
}
