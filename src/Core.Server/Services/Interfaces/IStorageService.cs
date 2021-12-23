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
        /// <returns>Task returning reader for the file or error</returns>
        Task<Result<BinaryReader, object>> GetFile(Guid accessId);
    }
}
