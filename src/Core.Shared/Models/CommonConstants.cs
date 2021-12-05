namespace Core.Shared.Models
{
    /// <summary>
    /// Constants used in backend and frontend.
    /// </summary>
    public static class CommonConstants
    {
        /// <summary>
        /// The name for header containing access token.
        /// </summary>
        public const string HEADER_AUTHORIZATION_NAME = "Authorization";
        /// <summary>
        /// The format used for getting date range filter strings.
        /// It provides format for a single date only.
        /// </summary>
        public const string DATE_RANGE_FILTER_FORMAT = "yyyy-MM-dd";
        /// <summary>
        /// The database row ID of the default administrator.
        /// </summary>
        public const int DEFAULT_ADMIN_ID = 1;
    }
}
