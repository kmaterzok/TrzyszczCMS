using System.Collections.Generic;

namespace Core.Shared.Models.Auth
{
    /// <summary>
    /// The class containing information about signed in user.
    /// </summary>
    public class AuthUserInfo
    {
        /// <summary>
        /// Row ID of the authenticated user.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Username of the authorised user.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Name of the active role the user has been assigned to.
        /// </summary>
        public string AssignedRoleName { get; set; }
        /// <summary>
        /// Row ID of the active role the user has been assigned to.
        /// </summary>
        public int AssignedRoleId { get; set; }
        /// <summary>
        /// Names of active permissions (policies) that owns this user.
        /// </summary>
        public List<string> AssignedPoliciesNames { get; set; }
        /// <summary>
        /// Access token for authentication.
        /// </summary>
        public string AccessToken { get; set; }

    }
}
