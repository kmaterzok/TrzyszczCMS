using System;

namespace TrzyszczCMS.Client.Helpers.Extensions
{
    /// <summary>
    /// THe extensions methods for <see cref="DateTime"/> handling.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Truncate hours, minutes and seconds. Leave the date part.
        /// </summary>
        /// <param name="source">Date and time</param>
        /// <returns>Only date</returns>
        public static DateTime TruncateHMS(this DateTime source) =>
            new DateTime(source.Year, source.Month, source.Day);

        /// <summary>
        /// Get hour, minute and second part of <see cref="DateTime"/>.
        /// </summary>
        /// <param name="source">Date and time</param>
        /// <returns>Only time</returns>
        public static TimeSpan GetHMS(this DateTime source) =>
            new TimeSpan(source.Hour, source.Minute, source.Second);
    }
}
