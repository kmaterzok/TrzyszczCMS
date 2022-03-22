using TrzyszczCMS.Core.Server.Helpers;
using TrzyszczCMS.Core.Server.Helpers.Extensions;
using TrzyszczCMS.Core.Server.Models;
using TrzyszczCMS.Core.Server.Models.Adapters;
using TrzyszczCMS.Core.Server.Models.Enums;
using TrzyszczCMS.Core.Server.Services.Interfaces;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers.Extensions;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageFiles;
using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using TrzyszczCMS.Core.Infrastructure.Models.Database;
using TrzyszczCMS.Core.Infrastructure.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Implementation.DbAccess.Modify
{
    public class ManageFileDbService : IManageFileDbService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _DatabaseStrategy;
        /// <summary>
        /// Used for uploading files.
        /// </summary>
        private readonly IStorageService _storageService;
        #endregion

        #region Ctor
        public ManageFileDbService(IDatabaseStrategyFactory databaseStrategyFactory, IStorageService storageService)
        {
            this._DatabaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
            this._storageService = storageService;
        }
        #endregion

        public async Task<DataPage<SimpleFileInfo>> GetSimpleFileInfoPage(int? fileNodeId, int desiredPageNumber, [NotNull] Dictionary<FilteredGridField, string> filters)
        {
            using (var ctx = _DatabaseStrategy.GetContext())
            {
                int allPagesCount = await ctx.ContFiles.AsNoTracking()
                                                       .Where(i => i.ParentFileId == fileNodeId)
                                                       .ApplyFilters(filters)
                                                       .CountAsync();

                int skippedPages = (desiredPageNumber - 1) * Constants.PAGINATION_PAGE_INFO_SIZE;

                var entries = await ctx.ContFiles.AsNoTracking()
                                                 .Where(i => i.ParentFileId == fileNodeId)
                                                 .ApplyFilters(filters)
                                                 .OrderBy(i => i.Name)
                                                 .Skip(skippedPages)
                                                 .Take(Constants.PAGINATION_PAGE_INFO_SIZE)
                                                 .Select(i => new SimpleFileInfo()
                                                 {
                                                     Id                   = i.Id,
                                                     AccessGuid           = i.AccessGuid,
                                                     CreationUtcTimestamp = i.CreationUtcTimestamp,
                                                     IsDirectory          = i.IsDirectory,
                                                     Name                 = i.Name,
                                                     ParentFileId         = i.ParentFileId,
                                                     MimeType             = i.MimeType
                                                 }).ToListAsync();

                if (skippedPages == 0 && fileNodeId.HasValue)
                {
                    var goUpDirectory = await ctx.ContFiles.SingleAsync(i => i.Id == fileNodeId.Value);
                    entries.Insert(0, new SimpleFileInfo()
                    {
                        Id                   = goUpDirectory.Id,
                        AccessGuid           = goUpDirectory.AccessGuid,
                        CreationUtcTimestamp = goUpDirectory.CreationUtcTimestamp,
                        IsDirectory          = true,
                        Name                 = "..",
                        ParentFileId         = goUpDirectory.ParentFileId,
                        MimeType             = null
                    });
                }

                return new DataPage<SimpleFileInfo>()
                {
                    Entries = entries,
                    PageNumber = desiredPageNumber,
                    HasPreviousPage = desiredPageNumber > 1,
                    HasNextPage = (skippedPages + entries.Count) < allPagesCount
                };
            }

        }
        public async Task<bool> DeleteFileAsync(int fileId)
        {
            using (var ctx = _DatabaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedOne = await ctx.ContFiles.SingleOrDefaultAsync(i => i.Id == fileId);
                    if (removedOne == null)
                    {
                        return false;
                    }
                    var deletedFiles = await GetFileAccessIdsRecursively(ctx, removedOne.Id);
                    ctx.ContFiles.Remove(removedOne);


                    await ctx.SaveChangesAsync();
                    if (removedOne.IsDirectory)
                    {
                        this._storageService.DeleteFiles(deletedFiles);
                    }
                    else
                    {
                        this._storageService.DeleteFile(removedOne.AccessGuid);
                    }
                    await ts.CommitAsync();
                    return true;
                }
            }
        }

        public async Task<Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>> CreateLogicalDirectoryAsync(string name, int? currentParentNodeId)
        {
            using (var ctx = _DatabaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    bool directoryExists = await ctx.ContFiles.AnyAsync(i =>
                        i.ParentFileId == currentParentNodeId && i.Name == name
                    );
                    if (directoryExists)
                    {
                        return Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>.MakeError(new Tuple<CreatingRowFailReason>(CreatingRowFailReason.AlreadyExisting));
                    }
                    else if (name == "..")
                    {
                        return Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>.MakeError(new Tuple<CreatingRowFailReason>(CreatingRowFailReason.CreatingForbidden));
                    }
                    
                    var newDirectoryGuid = await this.GetGuidForNewFileAsync(ctx);
                    var addedDirectory = await ctx.ContFiles.AddAsync(new ContFile()
                    {
                        AccessGuid           = newDirectoryGuid,
                        CreationUtcTimestamp = DateTime.UtcNow,
                        IsDirectory          = true,
                        Name                 = name,
                        ParentFileId         = currentParentNodeId
                    });


                    await ctx.SaveChangesAsync();
                    var info = addedDirectory.ToSimpleFileInfo();
                    await ts.CommitAsync();
                    return Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>.MakeSuccess(info);
                }
            }
        }

        public async Task<Result<List<SimpleFileInfo>, Tuple<CreatingFileFailReason>>> UploadFiles(IEnumerable<IServerUploadedFile> files, int? currentParentNodeId)
        {
            var creatingFailReason = FileManagementHelper.CheckFilesBeforeUpload(files);
            if (creatingFailReason.HasValue)
            {
                return Result<List<SimpleFileInfo>, Tuple<CreatingFileFailReason>>.MakeError(
                    new Tuple<CreatingFileFailReason>(creatingFailReason.Value)
                );
            }

            var uploadedFiles = new List<SimpleFileInfo>();
            foreach (var file in files)
            {
                using (var ctx = _DatabaseStrategy.GetContext())
                {
                    using (var ts = await ctx.Database.BeginTransactionAsync())
                    {
                        var newFileGuid = await this.GetGuidForNewFileAsync(ctx);
                        this._storageService.PutFile(file, newFileGuid);

                        var mimeType  = file.ContentType;
                        
                        var addedFile = await ctx.ContFiles.AddAsync(new ContFile()
                        {
                            CreationUtcTimestamp = DateTime.UtcNow,
                            Name                 = file.FileName.Truncate(Constraints.ContFile.NAME),
                            IsDirectory          = false,
                            ParentFileId         = currentParentNodeId,
                            AccessGuid           = newFileGuid,
                            MimeType             = mimeType
                        });
                        await ctx.SaveChangesAsync();
                        uploadedFiles.Add(addedFile.ToSimpleFileInfo());
                        await ts.CommitAsync();
                    }
                }
            }
            return Result<List<SimpleFileInfo>, Tuple<CreatingFileFailReason>>.MakeSuccess(uploadedFiles);
        }

        public async Task<FileTypeCheckResult> FileIsGraphics(Guid fileAccessGuid)
        {
            using (var ctx = _DatabaseStrategy.GetContext())
            {
                var foundFile = await ctx.ContFiles.AsNoTracking()
                                                   .FirstOrDefaultAsync(i => i.AccessGuid == fileAccessGuid);
                if (foundFile == null)
                {
                    return FileTypeCheckResult.NotFound;
                }
                return MimeTypeHelper.IsGraphics(foundFile.MimeType) ?
                    FileTypeCheckResult.OK :
                    FileTypeCheckResult.NotApplicableMimeType;
            }
        }
        #region Helper methods
        /// <summary>
        /// Generate new guid for a new file.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <returns>Not used GUID for a new file</returns>
        private async Task<Guid> GetGuidForNewFileAsync(CmsDbContext context)
        {
            var newGuid = Guid.NewGuid();
            while (await context.ContFiles.AsNoTracking().AnyAsync(i => i.AccessGuid == newGuid))
            {
                newGuid = Guid.NewGuid();
            }
            return newGuid;
        }



        /// <summary>
        /// Retrieve all files that are assigned in tyhe logical directory.
        /// </summary>
        /// <param name="context">Database context for getting data</param>
        /// <param name="nodeId">The file ID that is a directory and stores files</param>
        /// <returns>Task returning a list of GUIDs of files assigned directly or indirectly
        /// (through other directories) to the desired node</returns>
        private async Task<List<Guid>> GetFileAccessIdsRecursively(CmsDbContext context, int nodeId)
        {
            List<Guid> fileGuids = new();
            Stack<ContFile> analysedDirectories = new();
            var sourceDirectory = await context.ContFiles.SingleAsync(i => i.Id == nodeId);
            analysedDirectories.Push(sourceDirectory);

            while (analysedDirectories.Count > 0)
            {
                var checkedNode = analysedDirectories.Pop();
                var dataInsideDirectory = context.ContFiles.AsNoTracking()
                                                           .Where(i => i.ParentFileId == checkedNode.Id);

                fileGuids.AddRange(dataInsideDirectory.Where(i => !i.IsDirectory)
                                                      .Select(i => i.AccessGuid)
                                                      .AsEnumerable());

                var nextDirectoriesToCheck = dataInsideDirectory.Where(i => i.IsDirectory);
                analysedDirectories.PushRange(nextDirectoriesToCheck);
            }
            return fileGuids;
        }

        #endregion
    }
}
