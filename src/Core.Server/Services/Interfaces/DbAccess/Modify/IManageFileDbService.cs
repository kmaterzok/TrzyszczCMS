﻿using TrzyszczCMS.Core.Server.Models.Adapters;
using TrzyszczCMS.Core.Server.Models.Enums;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify
{
    /// <summary>
    /// Interface for managing files.
    /// </summary>
    public interface IManageFileDbService
    {
        /// <summary>
        /// Get basic information about files.
        /// </summary>
        /// <param name="fileNodeId">The file ID that is a directory and stores files</param>
        /// <param name="desiredPageNumber">Number of first page that will be fetched</param>
        /// <param name="filters">Filters applied for results</param>
        /// <returns>Task returning a page of data</returns>
        Task<DataPage<SimpleFileInfo>> GetSimpleFileInfoPage(int? fileNodeId, int desiredPageNumber, [NotNull] Dictionary<FilteredGridField, string> filters);
        
        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="fileId">Row ID of the file entry</param>
        /// <returns>Task returning if the file was found and deleted</returns>
        Task<bool> DeleteFileAsync(int fileId);

        /// <summary>
        /// Create a logical directory in the database.
        /// </summary>
        /// <param name="name">Name of the directory</param>
        /// <param name="currentParentNodeId">The id of the node for which the folder is created</param>
        /// <returns>Task returning info about created directory or error if the folder exists.</returns>
        Task<Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>> CreateLogicalDirectoryAsync(string name, int? currentParentNodeId);

        /// <summary>
        /// Upload files into a server and add entries into the database.
        /// </summary>
        /// <param name="files">Added files</param>
        /// <param name="currentParentNodeId">Directory which the files are added for</param>
        /// <returns>Task returning info about added files or error if something goes wrong</returns>
        Task<Result<List<SimpleFileInfo>, Tuple<CreatingFileFailReason>>> UploadFiles(IEnumerable<IServerUploadedFile> files, int? currentParentNodeId);

        /// <summary>
        /// Check if a file with a specified <paramref name="fileAccessGuid"/> exists and is a graphics file.
        /// </summary>
        /// <param name="fileAccessGuid">Access GUID fo the checked file</param>
        /// <returns>Task returning if the access GUID is compliat</returns>
        Task<FileTypeCheckResult> FileIsGraphics(Guid fileAccessGuid);
    }
}
