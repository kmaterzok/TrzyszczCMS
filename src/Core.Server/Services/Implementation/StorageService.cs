using Core.Server.Models.Settings;
using Core.Server.Services.Interfaces;
using Core.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation
{
    public class StorageService : IStorageService
    {
        #region Fields
        private readonly StorageSettings _storageSettings;
        #endregion

        #region Ctor
        public StorageService(IOptions<StorageSettings> storageSettings) =>
            this._storageSettings  = storageSettings.Value;
        #endregion

        public async Task<Result<BinaryReader, object>> GetFile(Guid accessId)
        {
            var filename = accessId.ToString(CommonConstants.FILE_ACCESS_ID_FORMAT);
            var filePath = Path.Join(Path.GetFullPath(this._storageSettings.Path), filename);

            if (!File.Exists(filePath))
            {
                return Result<BinaryReader, object>.MakeError(new object());
            }

            return Result<BinaryReader, object>.MakeSuccess(
                new BinaryReader(File.OpenRead(filePath))
            );
        }
    }
}
