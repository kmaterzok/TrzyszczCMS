using Core.Shared.Enums;
using Core.Shared.Models.ManageUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess.Modify
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
        /// <returns>Task returning if the user was found</returns>
        Task<bool> DeleteUserAsync(int userId);
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
    }
}
