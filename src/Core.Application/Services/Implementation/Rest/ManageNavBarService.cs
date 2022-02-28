using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Models.ManageSettings;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class ManageNavBarService : IManageNavBarService
    {
        #region Fields
        private readonly HttpClient _authHttpClient;
        #endregion

        #region Ctor
        public ManageNavBarService(IHttpClientFactory httpClientFactory) =>
            this._authHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_AUTH_NAME);
        #endregion

        #region Methods
        public async Task<List<SimpleMenuItemInfo>> GetSimpleMenuItemInfos(int? parentItemId)
        {
            string uri = parentItemId.HasValue ?
                $"/ManageNavBar/SimpleMenuItemInfos/{parentItemId.Value}" :
                $"/ManageNavBar/SimpleMenuItemInfos";

            return (await this._authHttpClient.GetFromJsonAsync<List<SimpleMenuItemInfo>>(uri));
        }

        public async Task<SimpleMenuItemInfo> AddMenuItem(SimpleMenuItemInfo addedItem)
        {
            var response = await this._authHttpClient.PostAsJsonAsync($"/ManageNavBar/AddMenuItem", addedItem);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SimpleMenuItemInfo>();
        }

        public async Task DeleteItem(int deletedItemId) =>
            (await this._authHttpClient.DeleteAsync($"/ManageNavBar/DeleteMenuItem/{deletedItemId}")).EnsureSuccessStatusCode();

        public async Task SwapOrderNumbers(int firstNodeId, int secondNodeId) =>
            (await this._authHttpClient.GetAsync($"/ManageNavBar/SwapOrderNumbers/{firstNodeId}/{secondNodeId}")).EnsureSuccessStatusCode();
        #endregion
    }
}
