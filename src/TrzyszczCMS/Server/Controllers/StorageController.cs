using Core.Server.Services.Interfaces;
using Core.Server.Services.Interfaces.DbAccess.Read;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TrzyszczCMS.Server.Controllers
{
    [ApiController]
    [Route("Storage")]
    public class StorageController : Controller
    {
        #region Fields
        private readonly IStorageService _storageService;
        private readonly ILoadFileDbService _loadFileService;
        #endregion

        #region Ctor
        public StorageController(IStorageService storageService, ILoadFileDbService loadFileService)
        {
            this._loadFileService = loadFileService;
            this._storageService  = storageService;
        }
        #endregion

        #region Methods
        [HttpGet]
        [Route("[action]/{accessId}")]
        public async Task<ActionResult> GetFile(string accessId)
        {
            if (Guid.TryParse(accessId, out Guid parsedAccessGuid))
            {
                if (this._storageService.GetFile(parsedAccessGuid).GetValue(out BinaryReader fileReader, out object _))
                {
                    try
                    {
                        var mimeType = await this._loadFileService.GetMimeType(parsedAccessGuid);
                        return new FileContentResult(fileReader.ReadBytes((int)fileReader.BaseStream.Length), mimeType);
                    }
                    finally
                    {
                        fileReader.Close();
                        fileReader.Dispose();
                    }
                }
                return NotFound();
            }
            return BadRequest("Invalid GUID syntax.");
        }
        #endregion
    }
}
