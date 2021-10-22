using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using System.Threading.Tasks;

namespace Core.Application.Services.Interfaces.Rest
{
    /// <summary>
    /// The service for getting data for pages.
    /// </summary>
    public interface ILoadPageService
    {
        /// <summary>
        /// Fetch content for displaying for the page.
        /// </summary>
        /// <param name="type">Type of requested page</param>
        /// <param name="name">Name of page that is encapsulated in the URI</param>
        /// <returns>Page content for display</returns>
        Task<ModularPageContentResponse> GetPageContentAsync(PageType type, string name);
    }
}
