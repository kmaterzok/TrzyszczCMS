using Core.Server.Helpers.Extensions;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Models.ManageUser;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementation.DbAccess.Modify
{
    public class ManageUserDbService : IManageUserDbService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public ManageUserDbService(IDatabaseStrategyFactory databaseStrategyFactory)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
        }
        #endregion

        #region Methods
        public async Task<List<SimpleUserInfo>> GetSimpleUserInfo(Dictionary<FilteredGridField, string> filters)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                var users = await ctx.AuthUsers.AsNoTracking()
                                               .ApplyFilters(filters)
                                               .Select(usr => new SimpleUserInfo()
                                               {
                                                   Id               = usr.Id,
                                                   UserName         = usr.Username,
                                                   Description      = usr.Description,
                                                   AssignedRoleName = usr.AuthRole.Name
                                               }).ToListAsync();
                return users;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedOne = await ctx.AuthUsers.SingleOrDefaultAsync(i => i.Id == userId);
                    if (removedOne == null)
                    {
                        return false;
                    }

                    ctx.AuthUsers.Remove(removedOne);
                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();

                    return true;
                }
            }
        }

        public async Task<DetailedUserInfo> GetDetailedUserInfo(int userId)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                var query = from usr in ctx.AuthUsers.AsNoTracking()
                            join rl in ctx.AuthRoles.AsNoTracking() on usr.AuthRoleId equals rl.Id
                            where usr.Id == userId
                            select new DetailedUserInfo()
                            {
                                Id = usr.Id,
                                UserName = usr.Username,
                                Description = usr.Description,
                                AssignedRoleId = rl.Id
                            };
                var user = await query.SingleOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }

                user.AvailableRoles = await ctx.AuthRoles.AsNoTracking()
                                                         .ToSimpleRoleInfo()
                                                         .ToListAsync();
                return user;
            }
        }

        public async Task<List<SimpleRoleInfo>> GetSimpleRoleInfo()
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                return await ctx.AuthRoles.AsNoTracking()
                                          .ToSimpleRoleInfo()
                                          .ToListAsync();
            }
        }

        #endregion
    }
}
