using Core.Application.Helpers;
using Core.Application.Helpers.Extensions;
using Core.Application.Helpers.Interfaces;
using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using Core.Shared.Models.Rest.Requests.ManageFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class ManageFileService : IManageFileService
    {
        #region Fields
        private readonly HttpClient _authHttpClient;
        #endregion

        #region Ctor
        public ManageFileService(IHttpClientFactory httpClientFactory) =>
            this._authHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_AUTH_NAME);
        #endregion

        #region Methods
        public IPageFetcher<SimpleFileInfo> GetSimpleFileInfos(int? fileNodeId, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber = 1) =>
            new PageFetcher<SimpleFileInfo>(desiredPageNumber, i => GetDataPageHandlerAsync(fileNodeId, filters, i));

        public async Task DeleteFile(int fileId) =>
            (await this._authHttpClient.DeleteAsync($"/ManageFile/DeleteFile/{fileId}")).EnsureSuccessStatusCode();
        #endregion

        #region Helper methods
        /// <summary>
        /// A handler method for getting pages of data. Used by fetchers.
        /// </summary>
        /// <param name="type">Type of desired pages</param>
        /// <param name="desiredPageNumber">Number of the first page to be fetched</param>
        /// <param name="filters">Filters applied for search results</param>
        /// <returns>Task returning a page of data</returns>
        private async Task<DataPage<SimpleFileInfo>> GetDataPageHandlerAsync(int? fileNodeId, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber)
        {
            var response = await this._authHttpClient.PostAsJsonAsync(
                "/ManageFile/SimpleFileInfo",
                new SimpleFileInfoRequest()
                {
                    FileNodeId = fileNodeId,
                    Filters = filters,
                    PageNumber = desiredPageNumber
                }
            );
            return await response.ContentOrDefaultAsync<DataPage<SimpleFileInfo>>();
        }
        #endregion
    }
}
