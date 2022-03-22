using TrzyszczCMS.Core.Server.Helpers.Extensions;
using TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageSettings;
using TrzyszczCMS.Core.Infrastructure.Enums;
using TrzyszczCMS.Core.Infrastructure.Helpers.Interfaces;
using TrzyszczCMS.Core.Infrastructure.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Implementation.DbAccess.Modify
{
    /// <summary>
    /// The implementation of <see cref="IManageNavBarDbService"/>.
    /// </summary>
    public class ManageNavBarDbService : IManageNavBarDbService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public ManageNavBarDbService(IDatabaseStrategyFactory databaseStrategyFactory) =>
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
        #endregion

        #region Methods
        public async Task<List<SimpleMenuItemInfo>> GetSimpleMenuItemInfos(int? parentItemId)
        {
            using (var ctx = this._databaseStrategy.GetContext())
            {
                var items = await ctx.ContMenuItems.AsNoTracking()
                                                   .Where(i => i.ParentItemId == parentItemId)
                                                   .OrderBy(i => i.OrderNumber)
                                                   .Select(i => new SimpleMenuItemInfo()
                                                   {
                                                       Id           = i.Id,
                                                       Name         = i.Name,
                                                       ParentItemId = i.ParentItemId,
                                                       OrderNumber  = i.OrderNumber,
                                                       Uri          = i.Uri
                                                   })
                                                   .ToListAsync();

                if (parentItemId.HasValue)
                {
                    var goUpItem = await ctx.ContMenuItems.SingleAsync(i => i.Id == parentItemId.Value);
                    items.Insert(0, new SimpleMenuItemInfo()
                    {
                        Id           = goUpItem.Id,
                        Name         = "..",
                        OrderNumber  = goUpItem.OrderNumber,
                        ParentItemId = goUpItem.ParentItemId,
                        Uri          = goUpItem.Uri
                    });
                }
                return items;
            }
        }

        public async Task<Result<SimpleMenuItemInfo, object>> AddMenuItem(SimpleMenuItemInfo addedItem)
        {
            using (var ctx = this._databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var checkedParentNode = await ctx.ContMenuItems.AsNoTracking()
                                                   .SingleOrDefaultAsync(i => i.Id == addedItem.ParentItemId);

                    if ((checkedParentNode != null && checkedParentNode.ParentItemId.HasValue) || addedItem.Name == "..")
                    {
                        await ts.RollbackAsync();
                        return Result<SimpleMenuItemInfo, object>.MakeError();
                    }

                    var newOrderNumber = await this.GenerateNewOrderNumberAsync(ctx);
                    var addedData = await ctx.ContMenuItems.AddAsync(new ContMenuItem()
                    {
                        Uri          = addedItem.Uri,
                        Name         = addedItem.Name,
                        OrderNumber  = newOrderNumber,
                        ParentItemId = addedItem.ParentItemId
                    });

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return Result<SimpleMenuItemInfo, object>.MakeSuccess(addedData.ToSimpleMenuItemInfo());
                }
            }
        }

        public async Task<bool> DeleteItem(int deletedItemId)
        {
            using (var ctx = this._databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedItem = await ctx.ContMenuItems.SingleOrDefaultAsync(i => i.Id == deletedItemId);
                    if (removedItem == null)
                    {
                        await ts.RollbackAsync();
                        return false;
                    }
                    
                    ctx.ContMenuItems.Remove(removedItem);

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return true;
                }
            }
        }

        public async Task<bool> SwapOrderNumbers(int firstNodeId, int secondNodeId)
        {
            using (var ctx = this._databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var dbFirstNode  = await ctx.ContMenuItems.SingleOrDefaultAsync(i => i.Id == firstNodeId);
                    var dbSecondNode = await ctx.ContMenuItems.SingleOrDefaultAsync(i => i.Id == secondNodeId);
                    if (dbFirstNode == null || dbSecondNode == null)
                    {
                        await ts.RollbackAsync();
                        return false;
                    }

                    var swapCopy             = dbFirstNode.OrderNumber;
                    dbFirstNode.OrderNumber  = dbSecondNode.OrderNumber;
                    dbSecondNode.OrderNumber = swapCopy;

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return true;
                }
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Generate a new order number ofor ordering items in the menu.
        /// </summary>
        /// <param name="context">Dastabse context</param>
        /// <returns>New order number, usually for a new item</returns>
        private async Task<int> GenerateNewOrderNumberAsync(CmsDbContext context) =>
            await context.ContMenuItems.AsNoTracking().AnyAsync() ?
                await context.ContMenuItems.AsNoTracking().MaxAsync(i => i.OrderNumber) + 1 : 1;
        #endregion
    }
}
