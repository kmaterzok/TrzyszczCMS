using Core.Shared.Models;
using System;

namespace Core.Shared.Helpers.Extensions
{
    /// <summary>
    /// The class providing simplifying methods for <see cref="DateTime"/> usage.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert to the most generic date and time notation.
        /// </summary>
        /// <param name="timestamp">Processed date and time</param>
        /// <returns>Timestamp in the generic notation</returns>
        public static string ToGenericNotation(this DateTime timestamp) =>
            timestamp.ToString(LocalConstants.GENERIC_NOTATION_FORMAT);
        
        /// <summary>
        /// Add 23:59:59 hours to the time.
        /// </summary>
        /// <param name="source"><see cref="DateTime"/> value</param>
        /// <returns><see cref="DateTime"/> with hours added</returns>
        public static DateTime AddMaxHour(this DateTime source) => 
            source.AddDays(1).AddSeconds(-1);
    }
}
