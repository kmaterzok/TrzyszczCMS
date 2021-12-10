using Core.Shared.Enums;
using Core.Shared.Models;
using Core.Shared.Models.ManageUser;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Core.Application.Services.Interfaces.Rest
{
    public interface IManageUserService
    {
        /// <summary>
        /// Get basic info about available users.
        /// </summary>
        /// <param name="filters">Dictionary of data filters</param>
        /// <returns>Collection of information about available users</returns>
        Task<List<SimpleUserInfo>> GetSimpleUserInfo([NotNull] Dictionary<FilteredGridField, string> filters);
        /// <summary>
        /// Delete user from the database.
        /// </summary>
        /// <param name="userId">Row ID of the deleted user</param>
        /// <returns>Task of the executed operation</returns>
        Task DeleteUser(int userId);
        /// <summary>
        /// Get detailed information about a specific user.
        /// </summary>
        /// <param name="userId">Row ID of the user</param>
        /// <returns>Task returning user details</returns>
        Task<DetailedUserInfo> GetDetailedUserInfo(int userId);
        /// <summary>
        /// Get all available roles.
        /// </summary>
        /// <returns>List of available roles</returns>
        Task<List<SimpleRoleInfo>> GetSimpleRoleInfo();
        /// <summary>
        /// Check if the username has been already in use.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Task returning if the user name exists and is valid</returns>
        Task<Result<Tuple<bool>, string>> UserNameExists(string username);
        /// <summary>
        /// Add a user depending on dat in <paramref name="user"/>.
        /// </summary>
        /// <param name="user">Data of the added user</param>
        /// <returns>Task returning a password of the newly created user</returns>
        Task<string> AddUser(DetailedUserInfo user);
        /// <summary>
        /// Update the user's data in the database.
        /// </summary>
        /// <param name="user">Data of updated user</param>
        /// <returns>Task of the executed operation</returns>
        Task UpdateUser(DetailedUserInfo user);
        /// <summary>
        /// Revoke the token that has <paramref name="tokenId"/> ID.
        /// </summary>
        /// <param name="tokenId">Row ID of the token</param>
        /// <returns>Task executing the operation</returns>
        Task RevokeToken(int tokenId);
        /// <summary>
        /// Get all tokens belonging to the signed in user.
        /// </summary>
        /// <returns>Task returning tokens</returns>
        Task<List<SimpleTokenInfo>> GetOwnSimpleTokenInfo();
    }
}
