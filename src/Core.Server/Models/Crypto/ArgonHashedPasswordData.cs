namespace Core.Server.Models.Crypto
{
    /// <summary>
    /// Data about hashed password.
    /// </summary>
    public class ArgonHashedPasswordData
    {
        /// <summary>
        /// Salt for hashed password (without common salt)
        /// </summary>
        public byte[] PasswordDependentSalt { get; set; }
        /// <summary>
        /// Argon2 hash for password
        /// </summary>
        public byte[] HashedPassword { get; set; }
    }
}
