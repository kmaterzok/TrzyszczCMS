using System;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess.Read
{
    /// <summary>
    /// The service for getting data for files.
    /// </summary>
    public interface ILoadFileDbService
    {
        /// <summary>
        /// Get MIME type of desired file.
        /// </summary>
        /// <param name="fileGuid">GUID of the desired file</param>
        /// <returns>Task returning a MIME type</returns>
        Task<string> GetMimeType(Guid fileGuid);
    }
}
