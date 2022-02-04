namespace Core.Shared.Helpers.Extensions
{
    /// <summary>
    /// The class of methods extending <see cref="string"/> functionalities.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncate string to the specific length or lesser.
        /// </summary>
        /// <param name="value">Input text</param>
        /// <param name="maxLength">Max length of output string</param>
        /// <returns>Output string</returns>
        public static string Truncate(this string value, int maxLength)
        {
            var lesser = value.Length < maxLength ? value.Length : maxLength;
            return value.Substring(0, lesser);
        }
    }
}
