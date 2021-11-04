using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.ViewModels.PageContent.Modules;

namespace TrzyszczCMS.Client.Helpers
{
    public static class ResponseExtensions
    {
        /// <summary>
        /// Create ViewModel by page module content get from backend.
        /// </summary>
        /// <param name="pageContent">Page modules content</param>
        /// <returns>List of viewmodels with content for display.</returns>
        public static List<IModuleViewModelBase> CreateViewModels(this ModularPageContentResponse pageContent)
        {
            var viewModels = new List<IModuleViewModelBase>();
            foreach (var singleModuleData in pageContent.ModuleContents)
            {
                switch (singleModuleData.GetPageType())
                {
                    case PageModuleType.TextWall:

                        viewModels.Add(new TextWallModuleViewModel()
                        {
                            LeftAsideMarkDown = singleModuleData.TextWallModuleContent.LeftAsideMarkDownContent,
                            RightAsideMarkDown = singleModuleData.TextWallModuleContent.RightAsideMarkDownContent,
                            SectionMarkDown = singleModuleData.TextWallModuleContent.SectionMarkDownContent,
                            SectionWidth = singleModuleData.TextWallModuleContent.SectionWidth
                        });
                        break;
                }
            }
            return viewModels;
        }
    }
}
