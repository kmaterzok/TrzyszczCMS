using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models.Rest.Responses.PageContent;
using System.Collections.Generic;
using TrzyszczCMS.Client.ViewModels.PageContent.Modules;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.Helpers
{
    public static class ResponseExtensions
    {
        /// <summary>
        /// Create ViewModel by page module content get from backend.
        /// </summary>
        /// <param name="pageContent">Page modules content</param>
        /// <param name="loadPageService">Service for getting data of pages</param>
        /// <returns>List of viewmodels with content for display.</returns>
        public static List<ViewModelBase> CreateViewModels(this ModularPageContentResponse pageContent, ILoadPageService loadPageService)
        {
            var viewModels = new List<ViewModelBase>();
            foreach (var singleModuleData in pageContent.ModuleContents)
            {
                switch (singleModuleData.GetModuleType())
                {
                    case PageModuleType.TextWall:
                        viewModels.Add(new TextWallModuleViewModel(singleModuleData.TextWallModuleContent));
                        break;

                    case PageModuleType.HeadingBanner:
                        viewModels.Add(new HeadingBannerModuleViewModel(singleModuleData.HeadingBannerModuleContent));
                        break;

                    case PageModuleType.PostListing:
                        viewModels.Add(new PostListingModuleViewModel(singleModuleData.PostListingModuleContent, loadPageService));
                        break;

                    default:
                        throw ExceptionMaker.NotImplemented.ForHandling(singleModuleData.GetModuleType(), $"{nameof(singleModuleData)}.{nameof(singleModuleData.GetModuleType)}()");
                }
            }
            return viewModels;
        }
    }
}
