using Core.Shared.Models;
using System;

namespace Core.Application.Helpers
{
    /// <summary>
    /// Helper preparing data for filtering on the backend side.
    /// This data must be parsed by the helper on the other side.
    /// </summary>
    public static class FilterDataPreparer
    {
        /// <summary>
        /// Create a string of date range used for filtering data by dates on the other side.
        /// </summary>
        /// <param name="range">Date range that defines the filtering string</param>
        /// <returns>Filter string</returns>
        public static string MakeDateRangeFilterString(this ValueRange<DateTime?> range)
        {
            DateTime? from = range.Start;
            DateTime? to   = range.End;

            if (!from.HasValue && !to.HasValue)
            {
                return null;
            }
            else if (!from.HasValue)
            {
                return $"<= {to.Value.ToString(CommonConstants.DATE_RANGE_FILTER_FORMAT)}";
            }
            else if (!to.HasValue)
            {
                return $">= {from.Value.ToString(CommonConstants.DATE_RANGE_FILTER_FORMAT)}";
            }
            else
            {
                return $"{from.Value.ToString(CommonConstants.DATE_RANGE_FILTER_FORMAT)} - {to.Value.ToString(CommonConstants.DATE_RANGE_FILTER_FORMAT)}";
            }
        }
    }
}
