namespace Core.Application.Models
{
    /// <summary>
    /// Constants used in the containing library.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The name for unauthorised HTTP client used for authorising.
        /// </summary>
        public const string HTTP_CLIENT_ANON_NAME = "Application.Unauthorized";
        /// <summary>
        /// The name for unauthorised HTTP client used for authorising.
        /// </summary>
        public const string HTTP_CLIENT_AUTH_NAME = "Application.Authorized";
        /// <summary>
        /// The disposition type for backend. Used during file upload.
        /// </summary>
        public const string FILE_UPLOAD_DISPOSITION_TYPE = "form-data";
    }
}
