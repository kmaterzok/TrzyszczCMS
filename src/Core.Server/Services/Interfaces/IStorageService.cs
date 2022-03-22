using TrzyszczCMS.Core.Server.Models.Adapters;
using TrzyszczCMS.Core.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace TrzyszczCMS.Core.Server.Services.Interfaces
{
    /// <summary>
    /// The service interface for getting contents of files.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Get a stream for getting content of the file.
        /// </summary>
        /// <param name="accessId">GUID that lets access a very specific file.</param>
        /// <returns>The reader for the file or error</returns>
        Result<BinaryReader, object> GetFile(Guid accessId);
        /// <summary>
        /// Put a file in the storage.
        /// </summary>
        /// <param name="file">Uploaded file details instance</param>
        /// <param name="accessId">GUID that lets access the uploaded file</param>
        void PutFile(IServerUploadedFile file, Guid accessId);
        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="accessId">GUID that lets access a very specific file.</param>
        void DeleteFile(Guid accessId);
        /// <summary>
        /// Delete files.
        /// </summary>
        /// <param name="accessIds">GUIDs that let access very specific files.</param>
        void DeleteFiles(IEnumerable<Guid> accessIds);
    }
}
