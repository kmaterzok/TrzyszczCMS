using Core.Application.Helpers;
using Core.Application.Helpers.Extensions;
using Core.Application.Helpers.Interfaces;
using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models;
using Core.Shared.Models.LoadPage;
using Core.Shared.Models.Rest.Responses.PageContent;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Services.Implementation.Rest
{
    public class LoadPageService : ILoadPageService
    {
        #region Fields
        private readonly HttpClient _noauthHttpClient;
        #endregion

        #region Ctor
        public LoadPageService(IHttpClientFactory httpClientFactory)
        {
            this._noauthHttpClient = httpClientFactory.CreateClient(Constants.HTTP_CLIENT_ANON_NAME);
        }
        #endregion

        #region Methods
        public async Task<ModularPageContentResponse> GetPageContentAsync(PageType type, string name)
        {
            HttpResponseMessage response;

            switch (type)
            {
                case PageType.Article:
                case PageType.Post:
                    response = await this._noauthHttpClient.GetAsync($"Page/PageContent/{type}/{name}");
                    break;

                case PageType.HomePage:
                    response = await this._noauthHttpClient.GetAsync("Page/HomePageContent");
                    break;

                default:
                    throw ExceptionMaker.Argument.Unsupported(type, nameof(type));
            }
            return await response.ContentOrDefaultAsync<ModularPageContentResponse>();
        }

        public IPageFetcher<SimplePublicPostInfo> GetSimplePublicPostInfos(int desiredPageNumber = 1) =>
            new PageFetcher<SimplePublicPostInfo>(desiredPageNumber, i => GetDataPageOfSimplePublicPostInfoHandlerAsync(i));
        #endregion

        #region Helper methods
        /// <summary>
        /// A handler method for getting pages of data. Used by fetchers.
        /// </summary>
        /// <param name="desiredPageNumber">Number of the first page to be fetched</param>
        /// <returns>Task returning a page of data</returns>
        private async Task<DataPage<SimplePublicPostInfo>> GetDataPageOfSimplePublicPostInfoHandlerAsync(int desiredPageNumber) =>
            await this._noauthHttpClient.GetFromJsonAsync<DataPage<SimplePublicPostInfo>>($"/Page/PublicPostInfo/{desiredPageNumber}");
        #endregion
    }
}
