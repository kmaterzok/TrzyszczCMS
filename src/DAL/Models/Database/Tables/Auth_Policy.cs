namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class containing a policy (permission) data used in the application.
    /// </summary>
    public class Auth_Policy
    {
        /// <summary>
        /// Row ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the policy (permission)
        /// </summary>
        public string Name { get; set; }
    }
}
