using Core.Application.Helpers.Interfaces;
using Core.Shared.Enums;
using Core.Shared.Models.ManageFiles;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Core.Application.Services.Interfaces.Rest
{
    /// <summary>
    /// The interface used for manaigng files in the application.
    /// </summary>
    public interface IManageFileService
    {
        /// <summary>
        /// Get basic information about files
        /// </summary>
        /// <param name="fileNodeId">The file ID that is a directory and stores files</param>
        /// <param name="filters">Filters applied to fetched data</param>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <returns>The fetcher getting data from the backend</returns>
        IPageFetcher<SimpleFileInfo> GetSimpleFileInfos(int? fileNodeId, [NotNull] Dictionary<FilteredGridField, string> filters, int desiredPageNumber = 1);

        /// <summary>
        /// Delete a file from the database depending on its ID.
        /// </summary>
        /// <param name="fileId">ID of the deleted file</param>
        /// <returns>Task executing the operation</returns>
        Task DeleteFile(int fileId);
    }
}
