using Core.Server.Models.Adapters;
using Core.Shared.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces
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
        /// <returns>Task returning error info</returns>
        Task PutFileAsync(IServerUploadedFile file, Guid accessId);
        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="accessId">GUID that lets access a very specific file.</param>
        void DeleteFile(Guid accessId);
    }
}
