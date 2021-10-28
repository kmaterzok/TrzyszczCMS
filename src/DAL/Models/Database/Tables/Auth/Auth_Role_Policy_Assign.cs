namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class describing what policy belongs to the certain role.
    /// </summary>
    public class Auth_Role_Policy_Assign
    {
        /// <summary>
        /// Row ID of the role that has an assigned policy.
        /// </summary>
        public int Auth_RoleId { get; set; }

        /// <summary>
        /// Row ID of the policy that has been assigned to a role.
        /// </summary>
        public int Auth_PolicyId { get; set; }


        public Auth_Policy Auth_Policy { get; set; }
        public Auth_Role Auth_Role { get; set; }
    }
}
