namespace TrzyszczCMS.Core.Shared.Models.ManageUser
{
    /// <summary>
    /// The simple information about a single user.
    /// Used for grid listings.
    /// </summary>
    public class SimpleUserInfo
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
        /// Name of the role that this user represents.
        /// </summary>
        public string AssignedRoleName { get; set; }
    }
}
