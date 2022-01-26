using Core.Server.Helpers;
using Core.Server.Helpers.Extensions;
using Core.Server.Models;
using Core.Server.Models.Enums;
using Core.Server.Services.Interfaces;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models;
using Core.Shared.Models.ManageUser;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
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
        /// <summary>
        /// Used for generating passwords.
        /// </summary>
        private readonly ICryptoService _cryptoService;
        #endregion

        #region Ctor
        public ManageUserDbService(IDatabaseStrategyFactory databaseStrategyFactory, ICryptoService cryptoService)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
            this._cryptoService = cryptoService;
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

        public async Task<DeleteRowFailReason?> DeleteUserAsync(int userId)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedOne = await ctx.AuthUsers.SingleOrDefaultAsync(i => i.Id == userId);
                    if (removedOne == null)
                    {
                        return DeleteRowFailReason.NotFound;
                    }
                    else if (removedOne.Id == CommonConstants.DEFAULT_ADMIN_ID)
                    {
                        return DeleteRowFailReason.DeletingForbidden;
                    }
                    // TODO: Make sure the signed in user does not delete its own account.

                    ctx.AuthUsers.Remove(removedOne);
                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();

                    return null;
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

        public async Task<bool> UserNameExists(string username)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                return await ctx.AuthUsers.AnyAsync(i => i.Username == username);
            }
        }

        public async Task<Result<string, Tuple<bool>>> AddUserAsync(DetailedUserInfo user)
        {
            if (!await this.UsernameCompliesWithRulesAsync(user.UserName))
            {
                return Result<string, Tuple<bool>>.MakeError(new Tuple<bool>(false));
            }

            var generatedPassword = CryptoHelper.GenerateRandomString(Constants.ADD_USER_DEFAULT_PASSWORD_LENGTH);
            var passwordHashInfo = this._cryptoService.GenerateHashWithSalt(generatedPassword);
            var cryptoSettings = this._cryptoService.CryptoSettings;

            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    await ctx.AuthUsers.AddAsync(new AuthUser()
                    {
                        Description       = user.Description,
                        Username          = user.UserName,
                        AuthRoleId        = user.AssignedRoleId,
                        Argon2Iterations  = cryptoSettings.Argon2Password.Iterations,
                        Argon2MemoryCost  = cryptoSettings.Argon2Password.MemoryCost,
                        Argon2Parallelism = cryptoSettings.Argon2Password.Parallelism,
                        PasswordSalt      = passwordHashInfo.PasswordDependentSalt,
                        PasswordHash      = passwordHashInfo.HashedPassword
                    });
                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return Result<string, Tuple<bool>>.MakeSuccess(generatedPassword);
                }
            }
        }

        public async Task<bool> UpdateUserAsync(DetailedUserInfo user)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var editedUser = await ctx.AuthUsers.SingleOrDefaultAsync(i => i.Id == user.Id);
                    if (editedUser == null || !await this.UsernameCompliesWithRulesAsync(user.UserName, editedUser.Username))
                    {
                        return false;
                    }

                    editedUser.Username    = user.UserName;
                    editedUser.Description = user.Description;
                    if (user.Id != CommonConstants.DEFAULT_ADMIN_ID)
                    {
                        editedUser.AuthRoleId  = user.AssignedRoleId;
                    }

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                    return true;
                }
            }
        }

        public async Task<DeleteRowFailReason?> RevokeTokenAsync(int tokenId, int signedInUserId, string usersSessionToken)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var removedOne = await ctx.AuthTokens.SingleOrDefaultAsync(i => i.Id == tokenId);
                    if (removedOne == null)
                    {
                        return DeleteRowFailReason.NotFound;
                    }
                    else if (removedOne.AuthUserId != signedInUserId)
                    {
                        return DeleteRowFailReason.DeletingForbidden;
                    }
                    else if (_cryptoService.GenerateHashFromPlainAccessToken(usersSessionToken).SequenceEqual(removedOne.HashedToken))
                    {
                        return DeleteRowFailReason.DeletingOwnStuff;
                    }

                    ctx.AuthTokens.Remove(removedOne);
                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();

                    return null;
                }
            }
        }

        public async Task<List<SimpleTokenInfo>> OwnSimpleTokenInfoAsync(int signedInUserId, string usersSessionToken)
        {
            return await Task.Run(() =>
            {
                using (var ctx = _databaseStrategy.GetContext())
                {
                    var tokenData = ctx.AuthTokens.Where(i => i.AuthUserId == signedInUserId)
                                                  .Select(i => new
                                                  {
                                                      Id                    = i.Id,
                                                      UsedForCurrentSession = false,
                                                      UtcCreateTime         = i.UtcCreateTime,
                                                      UtcExpiryTime         = i.UtcExpiryTime,
                                                      HashedToken           = i.HashedToken
                                                  }).AsEnumerable();
                    return tokenData.Select(i => new SimpleTokenInfo()
                                    {
                                        Id                    = i.Id,
                                        UsedForCurrentSession = _cryptoService.GenerateHashFromPlainAccessToken(usersSessionToken)
                                                                              .SequenceEqual(i.HashedToken),
                                        UtcCreateTime         = i.UtcCreateTime,
                                        UtcExpiryTime         = i.UtcExpiryTime
                                    }).ToList();
                }
            });
        }

        public async Task ChangeUserPasswordAsync(int userId, string newPassword)
        {
            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var ts = await ctx.Database.BeginTransactionAsync())
                {
                    var cryptoSettings = this._cryptoService.CryptoSettings;
                    var passwordHashInfo = this._cryptoService.GenerateHashWithSalt(newPassword);
                    var user = await ctx.AuthUsers.SingleAsync(i => i.Id == userId);

                    user.Argon2Iterations  = cryptoSettings.Argon2Password.Iterations;
                    user.Argon2MemoryCost  = cryptoSettings.Argon2Password.MemoryCost;
                    user.Argon2Parallelism = cryptoSettings.Argon2Password.Parallelism;
                    user.PasswordSalt      = passwordHashInfo.PasswordDependentSalt;
                    user.PasswordHash      = passwordHashInfo.HashedPassword;

                    await ctx.SaveChangesAsync();
                    await ts.CommitAsync();
                }
            }
        }
        #endregion

        #region Helper methods
        private async Task<bool> UsernameCompliesWithRulesAsync(string newUsername, string oldUsername = null)
        {
            return RegexHelper.IsValidUserName(newUsername) &&
            (
                (!string.IsNullOrEmpty(oldUsername) && oldUsername == newUsername) ||
                    !await this.UserNameExists(newUsername)
            );
        }
        #endregion
    }
}
