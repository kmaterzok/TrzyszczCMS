using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.LoadPage;
using TrzyszczCMS.Core.Shared.Models.Rest.Responses.PageContent;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Read
{
    /// <summary>
    /// The service for getting data for pages.
    /// </summary>
    public interface ILoadPageDbService
    {
        /// <summary>
        /// Get page content for displaying in the browser.
        /// </summary>
        /// <param name="type">Type of the page</param>
        /// <param name="name">Name of the page</param>
        /// <returns>Task returning page content</returns>
        Task<ModularPageContentResponse> GetPageContentAsync(PageType type, string name);

        /// <summary>
        /// Get basic information about public posts.
        /// </summary>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <returns>The page of data about posts</returns>
        Task<DataPage<SimplePublicPostInfo>> GetSimplePublicPostInfoPage(int desiredPageNumber = 1);
    }
}
