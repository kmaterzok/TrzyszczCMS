using Core.Server.Models.Enums;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using Core.Shared.Models.Rest.Requests.ManageFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Server.Data.Adapters;

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

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<List<SimpleFileInfo>>> Upload(IFormCollection files) =>
            await UploadFilesAsync(files?.Files, null);

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]/{parentNodeId}")]
        public async Task<ActionResult<List<SimpleFileInfo>>> Upload(IFormCollection files, int parentNodeId) =>
            await UploadFilesAsync(files?.Files, parentNodeId);

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{fileAccessGuid}")]
        public async Task<ActionResult<FileTypeCheckResult>> FileIsGraphics(string fileAccessGuid)
        {
            if (Guid.TryParse(fileAccessGuid, out Guid parsedFileAccessGuid))
            {
                return Ok(await this._manageFileService.FileIsGraphics(parsedFileAccessGuid));
            }
            return BadRequest("The file access GUID has invalid format.");
        }
        #endregion


        #region Helper methods
        /// <summary>
        /// Upload files into the storage and store info aobut them in the database.
        /// </summary>
        /// <param name="files">Details about uploaded files</param>
        /// <param name="parentNodeId">ID of the node that holds the files</param>
        /// <returns>Task returning result info about uploaded files</returns>
        private async Task<ActionResult<List<SimpleFileInfo>>> UploadFilesAsync(IFormFileCollection files, int? parentNodeId)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("There are no files uploaded.");
            }

            var fileAdapters = files.Select(i => new ServerUploadedFileAdapter(i));
            if ((await this._manageFileService.UploadFiles(fileAdapters, parentNodeId))
                .GetValue(out List<SimpleFileInfo> fileList, out Tuple<CreatingFileFailReason> error))
            {
                return Ok(fileList);
            }
            else
            {
                switch (error.Item1)
                {
                    case CreatingFileFailReason.FileSizeTooLarge:
                        return BadRequest("At least one of the uploaded files is too large.");

                    default:
                        throw ExceptionMaker.NotImplemented.ForHandling(error, $"{nameof(error)}.{nameof(error.Item1)}");
                }
            }
        }
        /// <summary>
        /// Try to create a directory and retrive a info about it.
        /// </summary>
        /// <param name="directoryName">Name of a new directory</param>
        /// <param name="parentNodeId">ID of the node that holds the directory</param>
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
                        return Conflict("A directory with the specified name has already existed.");

                    case CreatingRowFailReason.CreatingForbidden:
                        return BadRequest("Cannot create a directory with the specified data.");

                    default:
                        throw ExceptionMaker.NotImplemented.ForHandling(error.Item1, $"{nameof(error)}.{nameof(error.Item1)}");
                }
            }
        }
        #endregion
    }
}
