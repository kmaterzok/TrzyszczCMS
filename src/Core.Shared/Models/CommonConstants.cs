﻿namespace TrzyszczCMS.Core.Shared.Models
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
        /// <summary>
        /// The format for stored and displayed file access names/IDs.
        /// </summary>
        public const string FILE_ACCESS_ID_FORMAT = "N";
        /// <summary>
        /// The maximum file size that is allowed for upload.
        /// </summary>
        public const int MAX_UPLOADED_FILE_LENGTH_MEGABYTES = 28;
        /// <summary>
        /// The maximum file size that is allowed for upload.
        /// </summary>
        public static int MAX_UPLOADED_FILE_LENGTH_BYTES => 1024 * 1024 * MAX_UPLOADED_FILE_LENGTH_MEGABYTES;
    }
}
