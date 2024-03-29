﻿using TrzyszczCMS.Core.Server.Models.Enums;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify
{
    /// <summary>
    /// Interface for managing users.
    /// </summary>
    public interface IManageUserDbService
    {
        /// <summary>
        /// Get basic info about available users.
        /// </summary>
        /// <param name="filters">Filters applied for results</param>
        /// <returns>Collection of information about available users</returns>
        Task<List<SimpleUserInfo>> GetSimpleUserInfo(Dictionary<FilteredGridField, string> filters);
        /// <summary>
        /// Delete the user by <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">Row ID of the deleted user</param>
        /// <returns>Task returning the reason why the user was not deleted. <c>null</c> - successful operation.</returns>
        Task<DeleteRowFailReason?> DeleteUserAsync(int userId);
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
        /// Check if any user holds <paramref name="username"/>.
        /// </summary>
        /// <param name="username">Checked username</param>
        /// <returns>Username has been used for a user.</returns>
        Task<bool> UserNameExists(string username);
        /// <summary>
        /// Add a user to the database.
        /// </summary>
        /// <param name="user">Information about user</param>
        /// <returns>Task returning a password for the new user or error flag</returns>
        Task<Result<string, Tuple<bool>>> AddUserAsync(DetailedUserInfo user);
        /// <summary>
        /// Update a user in the database.
        /// </summary>
        /// <param name="user">Information about user</param>
        /// <returns>Task returning if the update was successful</returns>
        Task<bool> UpdateUserAsync(DetailedUserInfo user);
        /// <summary>
        /// Revoke the user's by <paramref name="tokenId"/>.
        /// </summary>
        /// <param name="tokenId">Row ID of the revoked token</param>
        /// <param name="signedInUserId">Row ID of the signed in user that executes the operation</param>
        /// <param name="usersSessionToken">Access token of the signed in user that is used in the current session</param>
        /// <returns>Task returning the reason why the token was not deleted. <c>null</c> - successful operation.</returns>
        Task<DeleteRowFailReason?> RevokeTokenAsync(int tokenId, int signedInUserId, string usersSessionToken);
        /// <summary>
        /// Get information about session access tokens of the user by <paramref name="signedInUserId"/>.
        /// </summary>
        /// <param name="signedInUserId">Row ID of the signed in user that executes the operation</param>
        /// <param name="usersSessionToken">Access token of the signed in user that is used in the current session</param>
        /// <returns>Task returning the collection of token infos</returns>
        Task<List<SimpleTokenInfo>> OwnSimpleTokenInfoAsync(int signedInUserId, string usersSessionToken);
        /// <summary>
        /// Check if the <paramref name="password"/> is valid for a user identified by <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">Row ID of the user</param>
        /// <param name="password">Checked password</param>
        /// <returns>Task returning if <paramref name="password"/> is valid for <paramref name="userId"/></returns>
        Task<bool> PasswordMatches(int userId, string password);
        /// <summary>
        /// Change the password of the user identified by <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">Row ID of the user</param>
        /// <param name="newPassword">New password for the user</param>
        /// <returns>Task with execution result</returns>
        Task ChangeUserPasswordAsync(int userId, string newPassword);
    }
}
