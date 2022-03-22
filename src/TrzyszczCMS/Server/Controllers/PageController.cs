using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Read;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.LoadPage;
using TrzyszczCMS.Core.Shared.Models.Rest.Responses.PageContent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TrzyszczCMS.Server.Controllers
{
    [ApiController]
    [Route("Page")]
    public class PageController : ControllerBase
    {
        #region Fields
        private readonly ILoadPageDbService _loadPageService;
        #endregion

        #region Ctor
        public PageController(ILoadPageDbService loadPageService) =>
            this._loadPageService = loadPageService;
        #endregion

        #region Methods
        /// <summary>
        /// Get contents of the homepage.
        /// </summary>
        /// <returns>Homepage's data for display and preparation</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<ModularPageContentResponse>> HomePageContent()
        {
            var pageContent = await this._loadPageService.GetPageContentAsync(PageType.HomePage, null);
            return null != pageContent ? Ok(pageContent) : NotFound();
        }
        /// <summary>
        /// Get contents of the page stored in the database.
        /// </summary>
        /// <param name="pageType">Type of fetched page</param>
        /// <param name="name">Name of the fetched page</param>
        /// <returns>Page's data for display and preparation</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{pageType}/{name}")]
        public async Task<ActionResult<ModularPageContentResponse>> PageContent(PageType pageType, string name)
        {
            if (PageType.HomePage == pageType)
            {
                return BadRequest($"Use {nameof(HomePageContent)} instead of {nameof(PageContent)}");
            }

            var pageContent = await this._loadPageService.GetPageContentAsync(pageType, name);
            return null != pageContent ? Ok(pageContent) : NotFound();
        }
        /// <summary>
        /// Get page of information about publicly available posts.
        /// </summary>
        /// <param name="pageNumber">Number of the desired page</param>
        /// <returns>Page of data about posts</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{pageNumber}")]
        public async Task<ActionResult<DataPage<SimplePublicPostInfo>>> PublicPostInfo(int pageNumber)
        {
            var pageContent = await this._loadPageService.GetSimplePublicPostInfoPage(pageNumber);
            return pageContent.Entries != null && pageContent.Entries.Count > 0 ? Ok(pageContent) : NotFound();
        }

        #endregion
    }
}
