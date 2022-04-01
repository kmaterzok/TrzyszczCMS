using System.Collections.Generic;
using System.Text;

namespace TrzyszczCMS.Core.Shared.Helpers.Extensions
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
        /// <summary>
        /// Aggregate many strings into one string.
        /// </summary>
        /// <param name="source">Enumeration of strings</param>
        /// <param name="delimiter">String delimiting other parts of string.</param>
        /// <returns><see cref="StringBuilder"/> instance containing aggregated string</returns>
        public static StringBuilder Aggregate(this IEnumerable<string> source, string delimiter)
        {
            var sb = new StringBuilder();
            foreach (var textline in source)
            {
                sb.Append(textline)
                  .Append(delimiter);
            }
            sb.Remove(sb.Length - delimiter.Length, delimiter.Length);
            return sb;
        }
    }
}
