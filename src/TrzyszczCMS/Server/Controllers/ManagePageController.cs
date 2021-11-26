using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using Core.Shared.Models.Rest.Requests.ManagePages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

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
        public ManagePageController(IManagePageDbService managePageService)
        {
            this._managePageService = managePageService;
        }
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
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> PageUriNameExists(string checkedUriName) =>
            await this._managePageService.PageUriNameExists(checkedUriName);
        #endregion
    }
}
