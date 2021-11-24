using Core.Application.Helpers.Extensions;
using Core.Application.Models;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models.Rest.Requests.Auth;
using Core.Shared.Models.Rest.Responses.Auth;
using Core.Shared.Models.Rest.Responses.PageContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
        #endregion
    }
}
