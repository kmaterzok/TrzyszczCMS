using Core.Server.Models.Enums;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Helpers;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using Core.Shared.Models.Rest.Requests.ManageFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
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

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{directoryName}")]
        public async Task<ActionResult<SimpleFileInfo>> CreateDirectory(string directoryName) =>
            await CreateDirectoryAsync(directoryName, null);

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{directoryName}/{parentNodeId}")]
        public async Task<ActionResult<SimpleFileInfo>> CreateDirectory(string directoryName, int parentNodeId) =>
            await CreateDirectoryAsync(directoryName, parentNodeId);
        #endregion

        #region Helper methods
        /// <summary>
        /// Try to create a directory and retrive a info about it.
        /// </summary>
        /// <param name="directoryName">Name of a new directory</param>
        /// <param name="parentNodeId">ID of the node that hold the directory</param>
        /// <returns>Task returning result info about created directory</returns>
        private async Task<ActionResult<SimpleFileInfo>> CreateDirectoryAsync(string directoryName, int? parentNodeId)
        {
            var result = await this._manageFileService.CreateLogicalDirectoryAsync(directoryName, parentNodeId);
            if (result.GetValue(out SimpleFileInfo file, out Tuple<CreatingRowFailReason> error))
            {
                return Ok(file);
            }
            else
            {
                switch (error.Item1)
                {
                    case CreatingRowFailReason.AlreadyExisting:
                        return Conflict();

                    case CreatingRowFailReason.CreatingForbidden:
                        return BadRequest();

                    default:
                        throw ExceptionMaker.NotImplemented.ForHandling(error.Item1, $"{nameof(error)}.{nameof(error.Item1)}");
                }
            }
        }
        #endregion
    }
}
