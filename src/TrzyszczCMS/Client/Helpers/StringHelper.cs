using System.Collections.Generic;
using System.Text;

namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// The methods simplifying dealing with strings.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Produce string filled with <paramref name="character"/> repeated <paramref name="repeats"/> times.
        /// </summary>
        /// <param name="character">Repeated character</param>
        /// <param name="repeats">Quantity of character repeats</param>
        /// <returns>Produced sequence</returns>
        public static string ProduceCharSequence(char character, int repeats)
        {
            StringBuilder sb = new StringBuilder();
            for (; repeats > 0; --repeats)
            {
                sb.Append(character);
            }
            return sb.ToString();
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
