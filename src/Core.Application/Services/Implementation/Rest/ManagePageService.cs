using Core.Application.Helpers;
using Core.Application.Helpers.Extensions;
using Core.Application.Helpers.Interfaces;
using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using Core.Shared.Models.Rest.Requests.ManagePages;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class ManagePageService : IManagePageService
    {
        #region Fields
        private readonly HttpClient _authHttpClient;
        #endregion

        #region Ctor
        public ManagePageService(IHttpClientFactory httpClientFactory)
        {
            this._authHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_AUTH_NAME);
        }
        #endregion

        #region Methods
        public IPageFetcher<SimplePageInfo> GetSimplePageInfos(PageType type, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber = 1)
        {
            return new PageFetcher<SimplePageInfo>(desiredPageNumber, i => GetDataPageHandlerAsync(type, filters, i));
        }
        public async Task<DetailedPageInfo> GetDetailedPageInfo(int id)
        {
            return await this._authHttpClient.GetFromJsonAsync<DetailedPageInfo>($"/ManagePage/DetailedPageInfo/{id}");
        }
        public async Task<DetailedPageInfo> GetDetailedPageInfoOfHomepage()
        {
            return await this._authHttpClient.GetFromJsonAsync<DetailedPageInfo>($"/ManagePage/DetailedPageInfoOfHomepage");
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// A handler method for getting pages of data. Used by fetchers.
        /// </summary>
        /// <param name="type">Type of desired pages</param>
        /// <param name="desiredPageNumber">Number of the first page to be fetched</param>
        /// <param name="filters">Filters applied for search results</param>
        /// <returns>Task returning a page of data</returns>
        private async Task<DataPage<SimplePageInfo>> GetDataPageHandlerAsync(PageType type, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber)
        {
            var response = await this._authHttpClient.PostAsJsonAsync(
                "/ManagePage/SimplePageInfo",
                new SimplePageInfoRequest()
                {
                    Type = type,
                    Filters = filters,
                    PageNumber = desiredPageNumber
                }
            );
            return await response.ContentOrDefaultAsync<DataPage<SimplePageInfo>>();
        }
        #endregion
    }
}
