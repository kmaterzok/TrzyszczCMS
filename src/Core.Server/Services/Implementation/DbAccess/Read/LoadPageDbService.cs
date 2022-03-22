using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Read;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models.Rest.Responses.PageContent;
using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Core.Server.Helpers;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.LoadPage;
using TrzyszczCMS.Core.Server.Models;

namespace TrzyszczCMS.Core.Server.Services.Implementation.DbAccess.Read
{
    public class LoadPageDbService : ILoadPageDbService
    {
        #region Fields
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public LoadPageDbService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Read);
        }
        #endregion

        #region Methods
        public async Task<ModularPageContentResponse> GetPageContentAsync(PageType type, string name)
        {
            string queriedName = type == PageType.HomePage ? "homepage" : name;

            using (var ctx = _databaseStrategy.GetContext())
            {
                var rawValueOfType = (byte)type;
                var moduleInfos = await (from p in ctx.ContPages.AsNoTracking()
                                         join m in ctx.ContModules.AsNoTracking() on p.Id equals m.ContPageId
                                         where p.UriName == queriedName &&
                                            p.Type == rawValueOfType &&
                                            p.PublishUtcTimestamp <= DateTime.UtcNow
                                         select new ModuleTypeValueInfo()
                                         {
                                             Id = m.Id,
                                             Type = m.Type
                                         }).ToListAsync();

                return new ModularPageContentResponse()
                {
                    ModuleContents = await moduleInfos.GetModuleContentsAsync(ctx)
                };
            }
        }

        public async Task<DataPage<SimplePublicPostInfo>> GetSimplePublicPostInfoPage(int desiredPageNumber = 1)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                int allPagesCount = await ctx.ContPages.AsNoTracking()
                                                       .Where(i => i.Type == (byte)PageType.Post && i.PublishUtcTimestamp < DateTime.UtcNow)
                                                       .OrderByDescending(i => i.PublishUtcTimestamp)
                                                       .CountAsync();

                int skippedPages = (desiredPageNumber - 1) * Constants.PAGINATION_PAGE_PUBLIC_POST_INFO_SIZE;

                var entries = await ctx.ContPages.AsNoTracking()
                                                 .Where(i => i.Type == (byte)PageType.Post && i.PublishUtcTimestamp < DateTime.UtcNow)
                                                 .OrderByDescending(i => i.PublishUtcTimestamp)
                                                 .Skip(skippedPages)
                                                 .Take(Constants.PAGINATION_PAGE_PUBLIC_POST_INFO_SIZE)
                                                 .Select(i => new SimplePublicPostInfo()
                                                 {
                                                     Title               = i.Title,
                                                     UriName             = i.UriName,
                                                     PublishUtcTimestamp = i.PublishUtcTimestamp,
                                                     AuthorsInfo         = i.AuthorsInfo,
                                                     Description         = i.Description
                                                 }).ToListAsync();

                return new DataPage<SimplePublicPostInfo>()
                {
                    Entries = entries,
                    PageNumber = desiredPageNumber,
                    HasPreviousPage = desiredPageNumber > 1,
                    HasNextPage = (skippedPages + entries.Count) < allPagesCount
                };
            }
        }
        #endregion
    }
}
