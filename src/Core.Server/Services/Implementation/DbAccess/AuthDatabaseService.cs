using Core.Server.Services.Interfaces;
using Core.Server.Services.Interfaces.DbAccess;
using Core.Shared.Models.Auth;
using DAL.Enums;
using DAL.Helpers.Interfaces;
using DAL.Models.Database.Tables;
using Dapper;
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

            using (var conn = _databaseStrategy.GetDbConnection())
            {
                var tokenInfo = await conn.QuerySingleOrDefaultAsync<AuthUserInfo>(
                    @"SELECT usr.""Id"" AS ""UserId"", usr.""Username"" AS ""Username"",
                             rl.""Name"" AS ""AssignedRoleName"", rl.""Id"" AS ""AssignedRoleId""
                      FROM ""Auth_User"" usr
                      INNER JOIN ""Auth_Token"" tkn ON usr.""Id"" = tkn.""Auth_UserId""
                      INNER JOIN ""Auth_Role"" rl ON usr.""Auth_RoleId"" = rl.""Id""
                      WHERE tkn.""HashedToken"" = @HashedToken AND tkn.""UtcExpiryTime"" > @UtcTime;",
                    new { HashedToken = _cryptoService.GenerateHashFromPlainAccessToken(accessToken), UtcTime = DateTime.UtcNow }
                );

                if (tokenInfo == null) { return null; }

                tokenInfo.AccessToken = accessToken;
                var assignedPolicies = (await conn.QueryAsync(@"SELECT pl.""Name"" AS ""PolicyName"" FROM ""Auth_Policy"" pl
                                                                INNER JOIN ""Auth_Role_Policy_Assign"" ras ON pl.""Id"" = ras.""Auth_PolicyId""
                                                                WHERE ras.""Auth_RoleId"" = @AssignedRoleId;", new { AssignedRoleId = tokenInfo.AssignedRoleId }))
                                            .Select(i => (string)i.PolicyName).ToList();

                tokenInfo.AssignedPoliciesNames = assignedPolicies;

                return tokenInfo;
            }
        }
        public async Task<AuthUserInfo> GenerateAuthData(string username, string password, bool remember)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            using (var conn = _databaseStrategy.GetDbConnection())
            {
                var userInfo = await conn.QuerySingleOrDefaultAsync<Auth_User>
                    (@"SELECT usr.""Id"", usr.""PasswordHash"", usr.""PasswordSalt"", usr.""Auth_RoleId"",
                       usr.""Argon2Iterations"", usr.""Argon2Parallelism"", usr.""Argon2MemoryCost""
                       FROM ""Auth_User"" usr WHERE usr.""Username"" = @Username;", new { Username = username });

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
                using (var ts = conn.BeginTransaction())
                {
                    await conn.ExecuteAsync(@"INSERT INTO ""Auth_Token""(""Auth_UserId"", ""HashedToken"", ""UtcExpiryTime"")
                                              VALUES (@Auth_UserId, @HashedToken, @UtcExpiryTime);", dbAuthToken);
                    ts.Commit();
                }
                var roleName = (string)(await conn.QuerySingleAsync(@$"SELECT rl.""Name"" FROM ""Auth_Role"" rl WHERE rl.""Id"" = @Id;",
                    new { Id = userInfo.Auth_RoleId })).Name;

                var policiesNames = (await conn.QueryAsync($@"SELECT pl.""Name"" FROM ""Auth_Policy"" pl
                                                              INNER JOIN ""Auth_Role_Policy_Assign"" asg ON pl.""Id"" = asg.""Auth_PolicyId""
                                                              WHERE asg.""Auth_RoleId"" = @RoleId;", new { RoleId = userInfo.Auth_RoleId }))
                                               .Select(i => (string)i.Name).ToList();
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
            string sql = $@"DELETE FROM ""{nameof(Auth_Token)}""
                            WHERE ""{nameof(Auth_Token.Auth_UserId)}"" = @Id AND ""{nameof(Auth_Token.HashedToken)}"" = @HashedToken;";

            using (var conn = this._databaseStrategy.GetDbConnection())
            {
                using (var ts = conn.BeginTransaction())
                {
                    byte[] hashedToken = _cryptoService.GenerateHashFromPlainAccessToken(token);
                    await conn.ExecuteAsync(sql, new { Id = userId, HashedToken = hashedToken });
                    ts.Commit();
                }
            }
        }
        #endregion
    }
}
