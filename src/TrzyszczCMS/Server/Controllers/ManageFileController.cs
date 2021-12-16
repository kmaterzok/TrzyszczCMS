using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using Core.Shared.Models.Rest.Requests.ManageFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Server.Controllers
{
    [ApiController]
    [Route("ManageFile")]
    [Authorize]
    public class ManageFileController : ControllerBase
    {
        #region Fields
        private readonly IManageFileDbService _manageFileService;
        #endregion

        #region Ctor
        public ManageFileController(IManageFileDbService manageFileService)
        {
            this._manageFileService = manageFileService;
        }
        #endregion

        #region Methods
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<DataPage<SimpleFileInfo>>> SimpleFileInfo([FromBody][NotNull] SimpleFileInfoRequest request)
        {
            var pageContent = await this._manageFileService.GetSimpleFileInfoPage(request.FileNodeId, request.PageNumber, request.Filters);
            return null != pageContent ? Ok(pageContent) : NotFound();
        }

        [HttpDelete]
        [Produces("application/json")]
        [Route("[action]/{fileId}")]
        public async Task<ActionResult> DeleteFile(int fileId) =>
            await this._manageFileService.DeleteFileAsync(fileId) ? Ok() : NotFound();
        #endregion
    }
}
