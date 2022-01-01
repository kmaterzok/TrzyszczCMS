using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess.Read
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
    }
}
