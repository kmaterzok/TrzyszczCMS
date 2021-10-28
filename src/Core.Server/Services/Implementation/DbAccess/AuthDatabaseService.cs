using Core.Server.Services.Interfaces;
using Core.Server.Services.Interfaces.DbAccess;
using Core.Shared.Models.Auth;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Server.Services.Implementations.DbAccess
{
    public class AuthDatabaseService : IAuthDatabaseService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _databaseStrategy;
        /// <summary>
        /// Used for generating hashes and validating passwords.
        /// </summary>
        private readonly ICryptoService _cryptoService;
        #endregion

        #region Ctor
        public AuthDatabaseService(IDatabaseStrategyFactory databaseStrategyFactory, ICryptoService cryptoService)
        {
            this._databaseStrategy = databaseStrategyFactory.GetStrategy(ConnectionStringDbType.Modify);
            this._cryptoService = cryptoService;
        }
        #endregion

        #region Methods
        public async Task<AuthUserInfo> GetAuthData(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            var hashedToken = _cryptoService.GenerateHashFromPlainAccessToken(accessToken);
            var timestamp = DateTime.UtcNow;

            using (var ctx = _databaseStrategy.GetContext())
            {
                using (var conn = ctx.Database.GetDbConnection())
                {
                    var tokenInfo = await (from usr in ctx.Auth_User.AsNoTracking()
                                    join tkn in ctx.Auth_Token.AsNoTracking() on usr.Id equals tkn.Auth_UserId
                                    join rl in ctx.Auth_Role.AsNoTracking() on usr.Auth_RoleId equals rl.Id
                                    where tkn.HashedToken == hashedToken && tkn.UtcExpiryTime > timestamp
                                    select new AuthUserInfo()
                                    {
                                        UserId = usr.Id,
                                        Username = usr.Username,
                                        AssignedRoleName = rl.Name,
                                        AssignedRoleId = rl.Id
                                    }).SingleOrDefaultAsync();
                                    
                    
                    if (tokenInfo == null) { return null; }

                    tokenInfo.AccessToken = accessToken;

                    var assignedPolicies = await (from pl in ctx.Auth_Policy.AsNoTracking()
                                                  join ras in ctx.Auth_Role_Policy_Assign.AsNoTracking() on pl.Id equals ras.Auth_PolicyId
                                                  where ras.Auth_RoleId == tokenInfo.AssignedRoleId
                                                  select pl.Name).ToListAsync();

                    tokenInfo.AssignedPoliciesNames = assignedPolicies;

                    return tokenInfo;
                }
            }
        }
        public async Task<AuthUserInfo> GenerateAuthData(string username, string password, bool remember)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            using (var ctx = _databaseStrategy.GetContext())
            {
                var userInfo = await ctx.Auth_User.AsNoTracking().SingleOrDefaultAsync(i => i.Username == username);

                if (userInfo == null) { return null; }
                
                if (!this._cryptoService.PasswordValid(userInfo.PasswordHash, userInfo.PasswordSalt, password,
                                                       userInfo.Argon2Parallelism, userInfo.Argon2Iterations, userInfo.Argon2MemoryCost))
                    { return null; }

                var tokenVariants = _cryptoService.GenerateAccessToken();

                var dbAuthToken = new Auth_Token()
                {
                    Auth_UserId = userInfo.Id,
                    HashedToken = tokenVariants.HashedToken,
                    UtcExpiryTime = remember ? DateTime.UtcNow.AddDays(182) : DateTime.UtcNow.AddHours(2)
                };    
                using (var ts = ctx.Database.BeginTransaction())
                {
                    await ctx.Auth_Token.AddAsync(dbAuthToken);
                    await ctx.SaveChangesAsync();
                    ts.Commit();
                }
                
                var roleName = (await ctx.Auth_Role.AsNoTracking().SingleAsync(i => i.Id == userInfo.Auth_RoleId)).Name;

                var policiesNames = await (from pl in ctx.Auth_Policy.AsNoTracking()
                                           join asg in ctx.Auth_Role_Policy_Assign.AsNoTracking() on pl.Id equals asg.Auth_PolicyId
                                           where asg.Auth_RoleId == userInfo.Auth_RoleId
                                           select pl.Name).ToListAsync();

                return new AuthUserInfo()
                {
                    AccessToken = tokenVariants.GetPlainTokenForBrowserStorage(),
                    UserId = userInfo.Id,
                    Username = username,
                    AssignedRoleId = userInfo.Auth_RoleId,
                    AssignedRoleName = roleName,
                    AssignedPoliciesNames = policiesNames
                };
            }
        }

        public async Task RevokeAccessToken(int userId, string token)
        {
            byte[] hashedToken = _cryptoService.GenerateHashFromPlainAccessToken(token);

            using (var ctx = this._databaseStrategy.GetContext())
            {
                using (var ts = ctx.Database.BeginTransaction())
                {
                    var foundToken = await ctx.Auth_Token.FirstOrDefaultAsync(i => i.Id == userId && i.HashedToken == hashedToken);
                    if (foundToken != null)
                    {
                        ctx.Auth_Token.Remove(foundToken);
                    }
                    ts.Commit();
                }
            }
        }
        #endregion
    }
}
