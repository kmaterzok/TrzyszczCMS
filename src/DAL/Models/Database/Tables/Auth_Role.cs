namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class for storing role data.
    /// Roles are assigned to users of the application.
    /// </summary>
    public class Auth_Role
    {
        /// <summary>
        /// Row ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the role
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The flag determining if the role has been created
        /// during development process and cannot
        /// be removed at any circumstance.
        /// </summary>
        public bool FactoryRole { get; set; }
    }
}
