namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// The class describing the table of user data stored in the database.
    /// </summary>
    public class Auth_User
    {
        /// <summary>
        /// Row ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User's name (login) used for distinction.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// A show note about user left on the site for everyone.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Argon2 password hash
        /// </summary>
        public byte[] PasswordHash { get; set; }
        /// <summary>
        /// A salt used for generating Argon2 password hash
        /// </summary>
        public byte[] PasswordSalt { get; set; }
        /// <summary>
        /// Argon2 iterations of hashing
        /// </summary>
        public int Argon2Iterations { get; set; }
        /// <summary>
        /// Argon2 parallelism factor
        /// </summary>
        public int Argon2Parallelism { get; set; }
        /// <summary>
        /// Argon2 memory usage for hashing
        /// </summary>
        public int Argon2MemoryCost { get; set; }
        /// <summary>
        /// Row ID of the role the user has been assigned to.
        /// </summary>
        public int Auth_RoleId { get; set; }
    }
}
