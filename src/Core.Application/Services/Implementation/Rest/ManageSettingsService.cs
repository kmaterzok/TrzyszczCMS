using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Models.ManageSettings;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class ManageSettingsService : IManageSettingsService
    {
        #region Fields
        private readonly HttpClient _authHttpClient;
        #endregion

        #region Ctor
        public ManageSettingsService(IHttpClientFactory httpClientFactory)
        {
            this._authHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_AUTH_NAME);
        }
        #endregion

        #region Methods
        public async Task<List<SimpleMenuItemInfo>> GetSimpleMenuItemInfos(int? parentItemId)
        {
            string uri = parentItemId.HasValue ?
                $"/ManageSettings/SimpleMenuItemInfos/{parentItemId.Value}" :
                $"/ManageSettings/SimpleMenuItemInfos";

            return (await this._authHttpClient.GetFromJsonAsync<List<SimpleMenuItemInfo>>(uri));
        }

        public async Task<SimpleMenuItemInfo> AddMenuItem(SimpleMenuItemInfo addedItem)
        {
            var response = await this._authHttpClient.PostAsJsonAsync($"/ManageSettings/AddMenuItem", addedItem);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SimpleMenuItemInfo>();
        }

        public async Task DeleteItem(int deletedItemId) =>
            (await this._authHttpClient.DeleteAsync($"/ManageSettings/DeleteMenuItem/{deletedItemId}")).EnsureSuccessStatusCode();

        public async Task SwapOrderNumbers(int firstNodeId, int secondNodeId) =>
            (await this._authHttpClient.GetAsync($"/ManageSettings/SwapOrderNumbers/{firstNodeId}/{secondNodeId}")).EnsureSuccessStatusCode();
        #endregion
    }
}
