using ApplicationCore.Models.Auth;
using ApplicationCore.Services.Interfaces.DbAccess;
using DAL.Helpers.Interfaces;
using DAL.Models.Database.Tables;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Implementations.DbAccess
{
    public class AuthDatabaseService : IAuthDatabaseService
    {
        #region Fields
        /// <summary>
        /// Used for persisting data in the database.
        /// </summary>
        private readonly IDatabaseStrategy _databaseStrategy;
        #endregion

        #region Ctor
        public AuthDatabaseService(IDatabaseStrategy databaseStrategy)
        {
            this._databaseStrategy = databaseStrategy;
        }
        #endregion

        #region Methods
        public async Task<AuthUserInfo> GetAuthData(string accessToken)
        {
            using (var conn = _databaseStrategy.GetDbConnection())
            {
                var tokenInfo = await conn.QuerySingleOrDefaultAsync<AuthUserInfo>(
                    @"SELECT usr.""Id"" AS ""UserId"", usr.""Username"" AS ""Username"", tkn.""Token"" AS ""AccessToken""
                             rl.""Name"" AS ""AssignedRoleName"", rl.""Id"" AS ""AssignedRoleId""
                      FROM ""Auth_User"" usr
                      INNER JOIN ""Auth_Token"" tkn ON usr.""Id"" = tkn.""Auth_UserId""
                      INNER JOIN ""Auth_Role"" rl ON usr.""Auth_RoleId"" = rl.""Id""
                      WHERE tkn.""Token"" = @AccessToken AND tkn.UtcExpiryTime < timeszone('UTC', now());", new { AccessToken = accessToken });

                var assignedPolicies = conn.Query(@"SELECT pl.Name AS ""PolicyName"" FROM ""Auth_Policy"" pl
                                                    INNER JOIN ""Auth_Role_Policy_Assign"" ras ON pl.""Id"" = pl.""Auth_PolicyId""
                                                    WHERE pl.""Auth_RoleId"" = @AssignedRoleId;", new { AssignedRoleId = tokenInfo.AssignedRoleId })
                                            .Select(i => (string)i.PolicyName).ToList();

                tokenInfo.AssignedPoliciesNames = assignedPolicies;

                return tokenInfo;
            }
        }

        public async Task<AuthUserInfo> GenerateAuthData(string username, string password, bool remember)
        {
            using (var conn = _databaseStrategy.GetDbConnection())
            {
                var userInfo = await conn.QuerySingleOrDefaultAsync<Auth_User>
                    (@"SELECT usr.""Id"", usr.""PasswordHash"", usr.""PasswordSalt"", usr.""Auth_RoleId""
                       FROM ""Auth_User"" usr WHERE usr.""Username"" = @Username;", new { Username = username });

                if (userInfo == null) { return null; }
                
                // TODO: Validate password.
                bool passwordIsValid = true;
                if (!passwordIsValid) { return null; }

                var dbAuthToken = new Auth_Token()
                {
                    Auth_UserId = userInfo.Id,
                    Token = Guid.NewGuid().ToString("N"), // TODO: Change generating of token.
                    UtcExpiryTime = remember ? DateTime.UtcNow.AddDays(182) : DateTime.UtcNow.AddHours(2)
                };    
                using (var ts = conn.BeginTransaction())
                {
                    await conn.ExecuteAsync(@"INSERT INTO ""Auth_Token""(""Auth_UserId"", ""Token"", ""UtcExpiryTime"")
                                              VALUES (@Auth_UserId, @Token, @UtcExpiryTime);", dbAuthToken);
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
                    AccessToken = dbAuthToken.Token,
                    UserId = userInfo.Id,
                    Username = username,
                    AssignedRoleId = userInfo.Auth_RoleId,
                    AssignedRoleName = roleName,
                    AssignedPoliciesNames = policiesNames
                };
            }
        }
        #endregion
    }
}
