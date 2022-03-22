using TrzyszczCMS.Core.Application.Helpers;
using TrzyszczCMS.Core.Application.Helpers.Extensions;
using TrzyszczCMS.Core.Application.Helpers.Interfaces;
using TrzyszczCMS.Core.Application.Models;
using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManagePage;
using TrzyszczCMS.Core.Shared.Models.Rest.Requests.ManagePages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Application.Services.Implementation.Rest
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
        public IPageFetcher<SimplePageInfo> GetSimplePageInfos(PageType type, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber = 1) =>
            new PageFetcher<SimplePageInfo>(desiredPageNumber, i => GetDataPageHandlerAsync(type, filters, i));

        public async Task<DetailedPageInfo> GetDetailedPageInfo(int id) =>
            await this._authHttpClient.GetFromJsonAsync<DetailedPageInfo>($"/ManagePage/DetailedPageInfo/{id}");
        
        public async Task<DetailedPageInfo> GetDetailedPageInfoOfHomepage() =>
            await this._authHttpClient.GetFromJsonAsync<DetailedPageInfo>($"/ManagePage/DetailedPageInfoOfHomepage");

        public async Task<Result<Tuple<bool>,string>> PageUriNameExists(string checkedUriName)
        {
            if (!RegexHelper.IsValidPageUriName(checkedUriName))
            {
                return Result<Tuple<bool>, string>.MakeError("PatternMismatch");
            }
            var checkResponse = await this._authHttpClient.GetAsync($"/ManagePage/PageUriNameExists/{checkedUriName}");
            return Result<Tuple<bool>, string>.MakeSuccess(new Tuple<bool>(checkResponse.IsSuccessStatusCode));
        }

        public async Task AddPage(DetailedPageInfo page) =>
            (await this._authHttpClient.PostAsJsonAsync($"/ManagePage/AddPage", page)).EnsureSuccessStatusCode();

        public async Task UpdatePage(DetailedPageInfo page) =>
            (await this._authHttpClient.PostAsJsonAsync($"/ManagePage/UpdatePage", page)).EnsureSuccessStatusCode();

        public async Task DeletePages(params int[] pageIds) =>
            (await this._authHttpClient.PostAsJsonAsync($"/ManagePage/DeletePages", pageIds)).EnsureSuccessStatusCode();
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
