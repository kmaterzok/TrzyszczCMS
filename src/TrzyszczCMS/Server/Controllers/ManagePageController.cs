using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using Core.Shared.Models.Rest.Requests.ManagePages;
using DAL.Shared.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TrzyszczCMS.Server.Data.Enums;
using TrzyszczCMS.Server.Helpers.Extensions;

namespace TrzyszczCMS.Server.Controllers
{
    [ApiController]
    [Route("ManagePage")]
    [Authorize]
    public class ManagePageController : ControllerBase
    {
        #region Fields
        private readonly IManagePageDbService _managePageService;
        #endregion

        #region Ctor
        public ManagePageController(IManagePageDbService managePageService) =>
            this._managePageService = managePageService;
        #endregion

        #region Methods
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<DataPage<SimplePageInfo>>> SimplePageInfo([FromBody][NotNull] SimplePageInfoRequest request)
        {
            var pageContent = await this._managePageService.GetSimplePageInfoPage(request.Type, request.PageNumber, request.Filters);
            return null != pageContent ? Ok(pageContent) : NotFound();
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{id}")]
        public async Task<ActionResult<DetailedPageInfo>> DetailedPageInfo(int id)
        {
            var pageDetails = await this._managePageService.GetDetailedPageInfo(id);
            return null != pageDetails ? Ok(pageDetails) : NotFound();
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<DetailedPageInfo>> DetailedPageInfoOfHomepage()
        {
            var pageDetails = await this._managePageService.GetDetailedPageInfoOfHomepage();
            return null != pageDetails ? Ok(pageDetails) : NotFound();
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{checkedUriName}")]
        public async Task<ActionResult> PageUriNameExists(string checkedUriName) =>
            await this._managePageService.PageUriNameExists(checkedUriName) ? Ok() : NotFound();

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult> AddPage([FromBody][NotNull] DetailedPageInfo request)
        {
            var pageType = (await this._managePageService.GetDetailedPageInfo(request.Id))?.PageType;
            if (!pageType.HasValue)
            {
                throw ExceptionMaker.Argument.Unsupported(request.Id, $"{nameof(request)}.{nameof(request.Id)}");
            }
            var policyName = pageType.Value.GetUserPolicyName(PageOperationType.Creating);

            return HttpContext.HasUserPolicy(policyName) ?
                (await this._managePageService.AddPageAsync(request) ? this.ObjectCreated() : Conflict("Some invalid data specified.")) :
                Forbid($"You have no permission to update the {pageType.Value.ToString().ToLower()}.");
        }

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult> UpdatePage([FromBody][NotNull] DetailedPageInfo request)
        {
            var pageType = (await this._managePageService.GetDetailedPageInfo(request.Id))?.PageType;
            if (!pageType.HasValue)
            {
                throw ExceptionMaker.Argument.Unsupported(request.Id, $"{nameof(request)}.{nameof(request.Id)}");
            }
            var policyName = pageType.Value.GetUserPolicyName(PageOperationType.Editing);
            
            return HttpContext.HasUserPolicy(policyName) ?
                (await this._managePageService.UpdatePageAsync(request) ? Ok() : Conflict("Some invalid or repetitive data specified.")) :
                Forbid($"You have no permission to update the {pageType.Value.ToString().ToLower()}.");
        }

        /// <summary>
        /// Delete pages from the database.
        /// </summary>
        /// <param name="pageIds">IDs of the deleted pages</param>
        /// <returns>Task returning HTTP response</returns>
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult> DeletePages([FromBody] int[] pageIds)
        {
            var pageCheckForArticles = await this._managePageService.AreAllPagesOfTypeAsync(PageType.Article, pageIds);
            var pageCheckForPosts    = await this._managePageService.AreAllPagesOfTypeAsync(PageType.Post,    pageIds);

            if (!pageCheckForArticles && pageCheckForPosts)
            {
                return HttpContext.HasUserPolicy(UserPolicies.BLOG_POST_DELETING) ?
                    (await this._managePageService.DeletePagesAsync(pageIds) ? Ok() : NotFound()) :
                    Forbid("You have no permission to delete any post.");
            }
            else if (pageCheckForArticles && !pageCheckForPosts)
            {
                return HttpContext.HasUserPolicy(UserPolicies.ARTICLE_DELETING) ?
                    (await this._managePageService.DeletePagesAsync(pageIds) ? Ok() : NotFound()) :
                    Forbid("You have no permission to delete any article.");
            }
            return Conflict("The deleted pages must be of one specific type - article or post.");
        }
        #endregion
    }
}
