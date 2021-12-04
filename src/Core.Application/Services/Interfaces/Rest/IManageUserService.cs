using Core.Shared.Enums;
using Core.Shared.Models.ManageUser;
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
    }
}
