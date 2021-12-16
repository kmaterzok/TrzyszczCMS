using Core.Server.Helpers.Extensions;
using Core.Server.Models;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
    }
}
