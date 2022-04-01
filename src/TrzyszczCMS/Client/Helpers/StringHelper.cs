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
    }
}
