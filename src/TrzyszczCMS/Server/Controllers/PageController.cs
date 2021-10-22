using Core.Server.Services.Interfaces.DbAccess.Read;
using Core.Shared.Enums;
using Core.Shared.Models.Rest.Responses.PageContent;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public PageController(ILoadPageDbService loadPageService)
        {
            this._loadPageService = loadPageService;
        }
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
        #endregion
    }
}
