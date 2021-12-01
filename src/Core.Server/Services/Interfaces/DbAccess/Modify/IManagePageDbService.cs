using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess.Modify
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
        /// Update page in the database.
        /// </summary>
        /// <param name="page">Information about page</param>
        /// <returns>Task returning if the update was successful</returns>
        Task<bool> UpdatePageAsync(DetailedPageInfo page);
        /// <summary>
        /// Delete pages width specified IDs.
        /// </summary>
        /// <param name="pageIds">IDs of the deleted pages</param>
        /// <returns>Task returning if the update was successful</returns>
        Task DeletePagesAsync(IEnumerable<int> pageIds);
    }
}
