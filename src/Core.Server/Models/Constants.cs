namespace TrzyszczCMS.Core.Server.Models
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
        /// <summary>
        /// The quantity of page info entries per single paginated page.
        /// </summary>
        public const int PAGINATION_PAGE_INFO_SIZE = 100;
        /// <summary>
        /// Validity time of the access token that has to be valid for
        /// a short period of time (no remember option during sign in).
        /// </summary>
        public const int SHORT_TERM_ACCESS_TOKEN_VALIDITY_HOURS = 2;
        /// <summary>
        /// Validity time of the access token that has to be valid for
        /// a long period of time (remember option checked during sign in).
        /// </summary>
        public const int LONG_TERM_ACCESS_TOKEN_VALIDITY_DAYS = 182;
        /// <summary>
        /// Default length of the password
        /// that is generated for a newly created user.
        /// </summary>
        public const int ADD_USER_DEFAULT_PASSWORD_LENGTH = 24;
        /// <summary>
        /// The time between trials of revoking expored tokens.
        /// </summary>
        public const int REPETITIVE_CYCLE_PERIOD_FOR_ACCESS_TOKEN_REVOKE_MILLIS = 600000;
        /// <summary>
        /// The quantity of page info entries about public posts per single paginated page.
        /// </summary>
        public const int PAGINATION_PAGE_PUBLIC_POST_INFO_SIZE = 25;

    }
}
