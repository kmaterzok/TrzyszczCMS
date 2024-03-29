﻿using TrzyszczCMS.Core.Server.Models.Adapters;
using TrzyszczCMS.Core.Server.Models.Settings;
using TrzyszczCMS.Core.Server.Services.Interfaces;
using TrzyszczCMS.Core.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using TrzyszczCMS.TrzyszczCMS.Core.Server.Services.Interfaces;

namespace TrzyszczCMS.Core.Server.Services.Implementation
{
    /// <summary>
    /// The class for handling files.
    /// </summary>
    public class StorageService : IStorageService
    {
        #region Fields
        private readonly StorageSettings _storageSettings;
        private readonly ILogger<IStorageService> _logger;
        private readonly IFileFacade _fileFacade;
        #endregion

        #region Ctor
        public StorageService(IOptions<StorageSettings> storageSettings, ILogger<IStorageService> logger, IFileFacade fileFacade)
        {
            this._storageSettings = storageSettings.Value;
            this._logger = logger;
            this._fileFacade = fileFacade;
        }
        #endregion

        #region Methods
        public Result<BinaryReader, object> GetFile(Guid accessId)
        {
            var filePath = this.MakeFullFilePath(accessId);
            if (!_fileFacade.Exists(filePath))
            {
                return Result<BinaryReader, object>.MakeError();
            }

            return Result<BinaryReader, object>.MakeSuccess(
                new BinaryReader(_fileFacade.OpenReadAsStream(filePath))
            );
        }

        public void PutFile(IServerUploadedFile file, Guid accessId)
        {
            var targetFilePath = this.MakeFullFilePath(accessId);
            using (var writeStream = _fileFacade.GetFileStreamAsStream(targetFilePath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(writeStream);
                this._logger.LogInformation($"File '{file.FileName}' of type {file.ContentType} and size {file.Length} was uploaded.");
            }
        }
        public void DeleteFile(Guid accessId)
        {
            var targetFilePath = this.MakeFullFilePath(accessId);
            if (_fileFacade.Exists(targetFilePath))
            {
                _fileFacade.Delete(targetFilePath);
                this._logger.LogInformation($"File identified by GUID {accessId} was deleted.");
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
