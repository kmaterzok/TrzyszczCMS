using Core.Server.Models.Adapters;
using Core.Server.Models.Settings;
using Core.Server.Services.Interfaces;
using Core.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
                return Result<BinaryReader, object>.MakeError();
            }

            return Result<BinaryReader, object>.MakeSuccess(
                new BinaryReader(File.OpenRead(filePath))
            );
        }

        public void PutFile(IServerUploadedFile file, Guid accessId)
        {
            var targetFilePath = this.MakeFullFilePath(accessId);
            using (var writeStream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(writeStream);
                // TODO: Add logging in the catch.
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

        public void DeleteFiles(IEnumerable<Guid> accessIds)
        {
            foreach (var accessId in accessIds)
            {
                this.DeleteFile(accessId);
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
