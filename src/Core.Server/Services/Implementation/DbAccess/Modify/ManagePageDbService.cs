using TrzyszczCMS.Core.Server.Helpers;
using TrzyszczCMS.Core.Server.Helpers.Extensions;
using TrzyszczCMS.Core.Server.Models;
using TrzyszczCMS.Core.Server.Models.Enums;
using TrzyszczCMS.Core.Server.Models.Extensions;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManagePage;
using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using TrzyszczCMS.Core.Infrastructure.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Implementation.DbAccess.Modify
{
    public class ManagePageDbService : IManagePageDbService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public ManagePageDbService(IDatabaseStrategyFactory databaseStrategyFactory) =>
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
        #endregion

        #region Methods
        public async Task<DataPage<SimplePageInfo>> GetSimplePageInfoPage(PageType type, int desiredPageNumber, [NotNull] Dictionary<FilteredGridField, string> filters)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                var typeValue = (byte)type;
                int allPagesCount = await ctx.ContPages.AsNoTracking()
                                                       .Where(i => i.Type == typeValue)
                                                       .ApplyFilters(filters)
                                                       .CountAsync();

                int skippedPages = (desiredPageNumber - 1) * Constants.PAGINATION_PAGE_INFO_SIZE;

                var entries = await ctx.ContPages.AsNoTracking()
                                                 .Where(i => i.Type == typeValue)
                                                 .ApplyFilters(filters)
                                                 .Skip(skippedPages)
                                                 .Take(Constants.PAGINATION_PAGE_INFO_SIZE)
                                                 .Select(i => new SimplePageInfo()
                                                 {
                                                     Id = i.Id,
                                                     Title = i.Title,
                                                     UriName = i.UriName,
                                                     CreateUtcTimestamp = i.CreateUtcTimestamp,
                                                     PublishUtcTimestamp = i.PublishUtcTimestamp
                                                 }).ToListAsync();

                return new DataPage<SimplePageInfo>()
                {
                    Entries = entries,
                    PageNumber = desiredPageNumber,
                    HasPreviousPage = desiredPageNumber > 1,
                    HasNextPage = (skippedPages + entries.Count) < allPagesCount
                };
            }
        }

        public async Task<DetailedPageInfo> GetDetailedPageInfo(int id)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                var rawPageInfo = await ctx.ContPages.AsNoTracking()
                                                     .FirstAsync(i => i.Id == id);
                
                var returnedDetails = new DetailedPageInfo()
                {
                    Id = rawPageInfo.Id,
                    PageType = (PageType)rawPageInfo.Type,
                    Title = rawPageInfo.Title,
                    UriName = rawPageInfo.UriName,
                    PublishUtcTimestamp = rawPageInfo.PublishUtcTimestamp,
                    AuthorsInfo = rawPageInfo.AuthorsInfo,
                    Description = rawPageInfo.Description
                };

                var moduleInfos = await ctx.ContModules.AsNoTracking()
                                                       .Where(i => i.ContPageId == id)
                                                       .Select(i => new ModuleTypeValueInfo()
                                                       {
                                                           Id   = i.Id,
                                                           Type = i.Type
                                                       }).ToListAsync();

                returnedDetails.ModuleContents = await moduleInfos.GetModuleContentsAsync(ctx);
                return returnedDetails;
            }
        }

        public async Task<DetailedPageInfo> GetDetailedPageInfoOfHomepage()
        {
            int homepageId;
            var homepageType = (byte)PageType.HomePage;
            
            using (var ctx = _databaseStrategy.GetContext())
            {
                homepageId = (await ctx.ContPages.AsNoTracking().FirstAsync(i => i.Type == homepageType)).Id;
            }
            return await this.GetDetailedPageInfo(homepageId);
        }

        public async Task<bool> PageUriNameExists(string checkedUriName)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                return await ctx.ContPages.AsNoTracking().AnyAsync(i => i.UriName == checkedUriName);
            }
        }

        public async Task<bool> AddPageAsync(DetailedPageInfo page)
        {
            if (!RegexHelper.IsValidPageUriName(page.UriName) ||
                page.PageType == PageType.HomePage)
            {
                return false;
            }

            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    if (await ctx.ContPages.AsNoTracking().AnyAsync(i => i.UriName == page.UriName))
                    {
                        await ts.RollbackAsync();
                        return false;
                    }

                    await ctx.ContPages.AddAsync(new ContPage()
                    {
                        UriName = page.UriName,
                        Type = (byte)page.PageType,
                        Title = page.Title,
                        PublishUtcTimestamp = page.PublishUtcTimestamp,
                        CreateUtcTimestamp = DateTime.UtcNow,
                        ContModules = page.ModuleContents.ToContModulesList(ctx),
                        AuthorsInfo = page.AuthorsInfo,
                        Description = page.Description
                    });

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return true;
                }
            }
        }
        
        public async Task<bool> UpdatePageAsync(DetailedPageInfo page)
        {
            if (!RegexHelper.IsValidPageUriName(page.UriName))
            {
                return false;
            }

            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    
                    var updatedData = await ctx.ContPages.SingleOrDefaultAsync(i => i.Id == page.Id);
                    if (updatedData == null)
                    {
                        await ts.RollbackAsync();
                        return false;
                    }
                    var uriWasUsed = await ctx.ContPages.AsNoTracking()
                                                        .Where(i => i.Id != page.Id)
                                                        .AnyAsync(i => i.UriName == page.UriName);
                    if (uriWasUsed)
                    {
                        await ts.RollbackAsync();
                        return false;
                    }

                    updatedData.Title = page.Title;
                    updatedData.UriName = page.UriName;
                    updatedData.PublishUtcTimestamp = page.PublishUtcTimestamp;
                    updatedData.AuthorsInfo = page.AuthorsInfo;
                    updatedData.Description = page.Description;

                    ctx.ContModules.RemoveRange(ctx.ContModules.Where(i => i.ContPageId == page.Id));
                    await ctx.SaveChangesAsync();


                    var addedModules = page.ModuleContents.ToContModulesList(ctx);
                    foreach (var module in addedModules)
                    {
                        module.ContPageId = page.Id;
                    }
                    await ctx.ContModules.AddRangeAsync(addedModules);

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return true;
                }
            }
        }

        public async Task<DeleteRowFailReason?> DeletePagesAsync(IEnumerable<int> pageIds)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedOnes = ctx.ContPages.Where(i => pageIds.Contains(i.Id));
                    if (await removedOnes.CountAsync() != pageIds.Count())
                    {
                        return DeleteRowFailReason.NotFound;
                    }
                    if (await removedOnes.AnyAsync(i => i.Type == (short)PageType.HomePage))
                    {
                        return DeleteRowFailReason.DeletingRequiredStuff;
                    }
                    ctx.ContPages.RemoveRange(removedOnes);

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return null;
                }
            }
        }

        public async Task<bool> AreAllPagesOfTypeAsync(PageType expectedPageType, params int[] pageIds)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                return await ctx.ContPages.Where(i => pageIds.Contains(i.Id)).AllAsync(i => i.Type == (short)expectedPageType);
            }
        }
        #endregion
    }
}
