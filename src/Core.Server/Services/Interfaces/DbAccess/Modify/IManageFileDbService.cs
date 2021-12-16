using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess.Modify
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
    }
}
