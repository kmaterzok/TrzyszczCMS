namespace TrzyszczCMS.Core.Shared.Models
{
    /// <summary>
    /// Constants used in <see cref="TrzyszczCMS.Core.Shared"/> project only.
    /// </summary>
    internal static class LocalConstants
    {
        /// <summary>
        /// The constant for generic notation for <see cref="DateTime"/> as <see cref="string"/>.
        /// </summary>
        public const string GENERIC_NOTATION_FORMAT = "yyyy-MM-dd HH:mm";
        /// <summary>
        /// The minimal required length of the password applied by a user for itself.
        /// </summary>
        public const int MIN_REQUIRED_USER_PASSWORD_LENGTH = 12;
    }
}
