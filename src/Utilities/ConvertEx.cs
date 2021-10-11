namespace Utilities
{
    /// <summary>
    /// Additional methods used for conversion data between encoding types.
    /// </summary>
    public static class ConvertEx
    {
        /// <summary>
        /// Replace <c>+</c> and <c>/</c> characters for URI safe characters in Base64 encoded data.
        /// </summary>
        /// <param name="originalBase64">Original unmodified Base64 string</param>
        /// <returns>URI safe Base64 string</returns>
        public static string Base64ReplaceToUriSafeChars(string originalBase64)
        {
            return originalBase64.Replace('+', '-')
                                 .Replace('/', '_');
        }

        /// <summary>
        /// Replace URI safe charascters with <c>+</c> and <c>/</c>
        /// </summary>
        /// <param name="safeBase64">URI safe Base64 string</param>
        /// <returns>Original unmodified Base64 string</returns>
        public static string Base64ReplaceFromUriSafeChars(string safeBase64)
        {
            return safeBase64.Replace('-', '+')
                             .Replace('_', '/');
        }
    }
}
