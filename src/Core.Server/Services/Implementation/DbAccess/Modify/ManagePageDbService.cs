using Core.Server.Helpers.Extensions;
using Core.Server.Models;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManagePage;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation.DbAccess.Modify
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
        public ManagePageDbService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
        }
        #endregion

        #region Methods
        public async Task<DataPage<SimplePageInfo>> GetSimplePageInfoPage(PageType type, int desiredPageNumber, [NotNull] Dictionary<FilteredGridField, string> filters)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                var typeValue = (byte)type;
                int allPagesCount = ctx.Cont_Page.AsNoTracking()
                                                 .Where(i => i.Type == typeValue)
                                                 .Count();

                int skippedPages = (desiredPageNumber - 1) * Constants.PAGINATION_PAGE_INFO_SIZE;

                var entries = await ctx.Cont_Page.AsNoTracking()
                                                 .Where(i => i.Type == typeValue)
                                                 .ApplyFilters(filters)
                                                 .Skip(skippedPages)
                                                 .Take(Constants.PAGINATION_PAGE_INFO_SIZE)
                                                 .Select(i => new SimplePageInfo()
                                                 {
                                                     Id = i.Id,
                                                     Title = i.Name,
                                                     CreateUtcTimestamp = i.CreateUtcTimestamp
                                                     // TODO: Displaying local time whilst using UTC on the server
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
        #endregion
    }
}
