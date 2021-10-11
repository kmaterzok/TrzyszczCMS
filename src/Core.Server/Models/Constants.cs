namespace Core.Server.Models
{
    /// <summary>
    /// The class holding cosnatns for usage in the project.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Quantity of bytes within the Argon2 hash that is generated.
        /// </summary>
        public const int ARGON_HASH_BYTES_QUANTITY = 64;
        /// <summary>
        /// Length of the salt used for every password.
        /// </summary>
        public const int ARGON_PASSWORD_SALT_LENGTH = 16;
        /// <summary>
        /// Quantity of bytes in the access token sent to the user.
        /// </summary>
        public const int ACCESS_TOKEN_BYTES_QUANTITY = 64;
    }
}
