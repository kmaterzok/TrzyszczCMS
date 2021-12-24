using Core.Server.Helpers.Extensions;
using Core.Server.Models;
using Core.Server.Models.Enums;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation.DbAccess.Modify
{
    public class ManageFileDbService : IManageFileDbService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public ManageFileDbService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
        }
        #endregion

        public async Task<DataPage<SimpleFileInfo>> GetSimpleFileInfoPage(int? fileNodeId, int desiredPageNumber, [NotNull] Dictionary<FilteredGridField, string> filters)
        {
            using (var ctx = _databaseStrategy.GetContext())
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
                                                     ParentFileId         = i.ParentFileId
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
                        ParentFileId         = goUpDirectory.ParentFileId
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
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedOne = await ctx.ContFiles.SingleOrDefaultAsync(i => i.Id == fileId);

                    if (removedOne == null)
                    {
                        return false;
                    }
                    ctx.ContFiles.Remove(removedOne);

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return true;
                }
            }
        }

        public async Task<Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>> CreateLogicalDirectoryAsync(string name, int? currentParentNodeId)
        {
            using (var ctx = _databaseStrategy.GetContext())
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
                        AccessGuid = newDirectoryGuid,
                        CreationUtcTimestamp = DateTime.UtcNow,
                        IsDirectory = true,
                        Name = name,
                        ParentFileId = currentParentNodeId
                    });


                    await ctx.SaveChangesAsync();

                    var info = new SimpleFileInfo()
                    {
                        Id                   = addedDirectory.Entity.Id,
                        AccessGuid           = addedDirectory.Entity.AccessGuid,
                        CreationUtcTimestamp = addedDirectory.Entity.CreationUtcTimestamp,
                        IsDirectory          = addedDirectory.Entity.IsDirectory,
                        Name                 = addedDirectory.Entity.Name,
                        ParentFileId         = addedDirectory.Entity.ParentFileId
                    };

                    await ts.CommitAsync();
                    return Result<SimpleFileInfo, Tuple<CreatingRowFailReason>>.MakeSuccess(info);
                }
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
        #endregion
    }
}
