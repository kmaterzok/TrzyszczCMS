using Core.Server.Models.Adapters;
using Core.Server.Models.Settings;
using Core.Server.Services.Interfaces;
using Core.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation
{
    /// <summary>
    /// The class for handling files.
    /// </summary>
    public class StorageService : IStorageService
    {
        #region Fields
        private readonly StorageSettings _storageSettings;
        #endregion

        #region Ctor
        public StorageService(IOptions<StorageSettings> storageSettings) =>
            this._storageSettings  = storageSettings.Value;
        #endregion

        #region Methods
        public Result<BinaryReader, object> GetFile(Guid accessId)
        {
            var filePath = this.MakeFullFilePath(accessId);
            if (!File.Exists(filePath))
            {
                return Result<BinaryReader, object>.MakeError(new object());
            }

            return Result<BinaryReader, object>.MakeSuccess(
                new BinaryReader(File.OpenRead(filePath))
            );
        }

        public async Task PutFileAsync(IUploadedFile file, Guid accessId)
        {
            var targetFilePath = this.MakeFullFilePath(accessId);
            using (var fileStream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    await file.CopyToAsync(fileStream);
                    // TODO: Add logging in the catch.
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        public void DeleteFile(Guid accessId)
        {
            var targetFilePath = this.MakeFullFilePath(accessId);
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Create a valid path for a stored file.
        /// </summary>
        /// <param name="accessId">GUID that lets access a very specific file.</param>
        /// <returns>Absolute path of the file</returns>
        private string MakeFullFilePath(Guid accessId) =>
            Path.Join(Path.GetFullPath(this._storageSettings.Path), accessId.ToString(CommonConstants.FILE_ACCESS_ID_FORMAT));
        #endregion
    }
}
