using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TrzyszczCMS.Core.Shared.Models.ManageUser
{
    /// <summary>
    /// All necessary info for managing and editing.
    /// </summary>
    public class DetailedUserInfo
    {
        /// <summary>
        /// Row ID of the page.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Login / user name of the user.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Additional description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Row ID of the role assigned to this user.
        /// </summary>
        public int AssignedRoleId { get; set; }
        /// <summary>
        /// Roles available to be assigned to this user
        /// </summary>
        public List<SimpleRoleInfo> AvailableRoles { get; set; }


        /// <summary>
        /// Create a new empty instance of details for creating a new user.
        /// </summary>
        /// <param name="availableRoles">Enumeration of all roles available to be assigned.</param>
        /// <returns>Empty instance</returns>
        public static DetailedUserInfo MakeEmpty([NotNull] IEnumerable<SimpleRoleInfo> availableRoles) => new DetailedUserInfo()
        {
            Id = 0,
            AvailableRoles = availableRoles.ToList(),
            AssignedRoleId = availableRoles.First().Id,
            Description = null,
            UserName = null,
        };


    }
}
