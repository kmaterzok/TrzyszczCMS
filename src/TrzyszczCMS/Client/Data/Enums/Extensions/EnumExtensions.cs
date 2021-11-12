using System;
using System.Linq;

namespace TrzyszczCMS.Client.Data.Enums.Extensions
{
    /// <summary>
    /// Adds methods for enums. Eases usage of enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get value of next enum defined in <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of iterated enum</typeparam>
        /// <param name="source">Predcessor of the desired value</param>
        /// <returns>Desired value</returns>
        public static T NextValue<T>(this T source) where T : Enum
        {
            var values = Enum.GetValues(typeof(T))
                             .Cast<T>()
                             .ToList();

            var foundIndex = values.IndexOf(source);
            var nextValueIndex = (foundIndex + 1) % values.Count;

            return values[nextValueIndex];
        }
    }
}
