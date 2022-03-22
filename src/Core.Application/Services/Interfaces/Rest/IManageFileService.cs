using TrzyszczCMS.Core.Application.Enums;
using TrzyszczCMS.Core.Application.Helpers.Interfaces;
using TrzyszczCMS.Core.Application.Models.Adapters;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Application.Services.Interfaces.Rest
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

        /// <summary>
        /// Create a logical directory in the database.
        /// </summary>
        /// <param name="name">Name of the directory</param>
        /// <param name="currentParentNodeId">The id of the node for which the folder is created</param>
        /// <returns>Task returning info about created directory or error if the folder exists.</returns>
        Task<Result<SimpleFileInfo, object>> CreateLogicalDirectory(string name, int? currentParentNodeId);

        /// <summary>
        /// Upload files onto the server.
        /// </summary>
        /// <param name="files">Uploaded files information</param>
        /// <param name="currentParentNodeId">A node which the files will be placed upon.</param>
        /// <param name="onTriedUpload">Action fired when a single upload trial is finished and the result object is sent</param>
        /// <returns></returns>
        Task UploadFiles(IEnumerable<IClientUploadedFile> files, int? currentParentNodeId, Action<Result<SimpleFileInfo, object>> onTriedUpload);

        /// <summary>
        /// Check if a file with a specified <paramref name="fileAccessGuid"/> exists and is a graphics file.
        /// </summary>
        /// <param name="fileAccessGuid">Access GUID fo the checked file</param>
        /// <returns>Task returning if the access GUID is compliat</returns>
        Task<FileTypeCheckResult> FileIsGraphics(string fileAccessGuid);
    }
}
