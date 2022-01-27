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
        /// Get a <see cref="DateTime"/> with an hour of value 23:59:59.
        /// </summary>
        /// <param name="source"><see cref="DateTime"/> value</param>
        /// <returns><see cref="DateTime"/> with hours added</returns>
        public static DateTime GetWithMaxHour(this DateTime source) =>
            new DateTime(source.Year, source.Month, source.Day, 23, 59, 59);
    }
}
