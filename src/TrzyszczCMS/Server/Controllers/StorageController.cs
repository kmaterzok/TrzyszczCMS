using Core.Server.Services.Interfaces;
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
        private readonly IStorageService _loadFileService;
        #endregion

        #region Ctor
        public StorageController(IStorageService loadFileService) =>
            this._loadFileService = loadFileService;
        #endregion

        #region Methods
        [HttpGet]
        [Route("[action]/{accessId}")]
        public async Task<ActionResult> GetFile(string accessId)
        {
            BinaryReader fileReader = null;
            if (Guid.TryParse(accessId, out Guid parsedAccessGuid) &&
                (await this._loadFileService.GetFile(parsedAccessGuid)).GetValue(out fileReader, out object _))
            {
                try
                {
                    return new FileContentResult(fileReader.ReadBytes((int)fileReader.BaseStream.Length), "application/octet-stream");
                }
                finally
                {
                    fileReader.Close();
                    fileReader.Dispose();
                }
            }
            return NotFound();
        }
        #endregion
    }
}
