using Core.Application.Helpers.Interfaces;
using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Core.Application.Services.Interfaces.Rest
{
    public interface IManagePageService
    {
        /// <summary>
        /// Get basic information about sites
        /// </summary>
        /// <param name="type">Type of fetched pages</param>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <param name="filters">Filters applied to fetched data</param>
        /// <returns>The fetcher getting data from the backend</returns>
        IPageFetcher<SimplePageInfo> GetSimplePageInfos(PageType type, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber = 1);
    }
}
