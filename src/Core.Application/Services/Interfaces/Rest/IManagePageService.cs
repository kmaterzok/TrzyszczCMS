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
        /// <param name="filters">Filters applied to fetched data</param>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <returns>The fetcher getting data from the backend</returns>
        IPageFetcher<SimplePageInfo> GetSimplePageInfos(PageType type, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber = 1);

        /// <summary>
        /// Get detailed information about a specific page.
        /// </summary>
        /// <param name="id">ID of the page</param>
        /// <returns>Task returning page details</returns>
        Task<DetailedPageInfo> GetDetailedPageInfo(int id);
        /// <summary>
        /// Get detailed information about the homepage.
        /// </summary>
        /// <returns>Task returning homepage details</returns>
        Task<DetailedPageInfo> GetDetailedPageInfoOfHomepage();
        /// <summary>
        /// Check if there is a URI name that has been already used for any existing page.
        /// </summary>
        /// <param name="checkedUriName">URI name to be checked</param>
        /// <returns>Task returning if the URI name is still in use or error info</returns>
        Task<Result<Tuple<bool>, string>> PageUriNameExists(string checkedUriName);

        /// <summary>
        /// Add a new page to the database.
        /// </summary>
        /// <param name="page">Page content</param>
        /// <returns>Task executing the operation</returns>
        Task AddPage(DetailedPageInfo page);

        /// <summary>
        /// Apply changes to a page in the database.
        /// </summary>
        /// <param name="page">Page content</param>
        /// <returns>Task executing the operation</returns>
        Task UpdatePage(DetailedPageInfo page);

        /// <summary>
        /// Delete pages from the database depending on their IDs.
        /// </summary>
        /// <param name="pageIds">IDs of the deleted pages</param>
        /// <returns>Task executing the operation</returns>
        Task DeletePages(params int[] pageIds);
    }
}
