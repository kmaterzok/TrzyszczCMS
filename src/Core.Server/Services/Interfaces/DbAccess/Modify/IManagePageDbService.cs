using TrzyszczCMS.Core.Server.Models.Enums;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManagePage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify
{
    public interface IManagePageDbService
    {
        /// <summary>
        /// Get basic information about sites.
        /// </summary>
        /// <param name="type">Type of fetched pages</param>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <param name="filters">Filters applied for results</param>
        /// <returns>Task returning a page of data</returns>
        Task<DataPage<SimplePageInfo>> GetSimplePageInfoPage(PageType type, int desiredPageNumber, Dictionary<FilteredGridField, string> filters);
        /// <summary>
        /// Get detailed information about a single site.
        /// </summary>
        /// <param name="id">Database row ID of the site</param>
        /// <returns>Task returning details about the site</returns>
        Task<DetailedPageInfo> GetDetailedPageInfo(int id);
        /// <summary>
        /// Get detailed information about the homepage.
        /// </summary>
        /// <returns>Task returning details about the homepage</returns>
        Task<DetailedPageInfo> GetDetailedPageInfoOfHomepage();
        /// <summary>
        /// Check if there is a URI name that has been already used for any existing page.
        /// </summary>
        /// <param name="checkedUriName">URI name to be checked</param>
        /// <returns>URI name is still in use</returns>
        Task<bool> PageUriNameExists(string checkedUriName);
        /// <summary>
        /// Add page to the database.
        /// </summary>
        /// <param name="page">Information about page</param>
        /// <returns>Task returning if the addition was successful</returns>
        Task<bool> AddPageAsync(DetailedPageInfo page);
        /// <summary>
        /// Update a page in the database.
        /// </summary>
        /// <param name="page">Information about page</param>
        /// <returns>Task returning if the update was successful</returns>
        Task<bool> UpdatePageAsync(DetailedPageInfo page);
        /// <summary>
        /// Delete a pages width specified IDs.
        /// </summary>
        /// <param name="pageIds">IDs of the deleted pages</param>
        /// <returns>Task returning if the deletion was successful</returns>
        Task<DeleteRowFailReason?> DeletePagesAsync(IEnumerable<int> pageIds);
        /// <summary>
        /// Check if all sites represented by their IDs in <paramref name="pageIds"/> are of type <paramref name="expectedPageType"/>.
        /// </summary>
        /// <param name="expectedPageType">The tyupe of page that is expected</param>
        /// <param name="pageIds">IDs of checked pages</param>
        /// <returns>Task returning if all desired pages are of the specified type</returns>
        Task<bool> AreAllPagesOfTypeAsync(PageType expectedPageType, params int[] pageIds);
    }
}
