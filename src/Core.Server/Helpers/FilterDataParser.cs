using Core.Shared.Helpers.Extensions;
using Core.Shared.Models;
using System;

namespace Core.Server.Helpers
{
    /// <summary>
    /// Helper parsing data for filtering.
    /// This data must be prepared by the helper on the other side.
    /// </summary>
    public static class FilterDataParser
    {
        /// <summary>
        /// Parse filter string into a date range.
        /// </summary>
        /// <param name="filterString">Filtering string</param>
        /// <returns>Date range</returns>
        public static ValueRange<DateTime?> ToDateRange(string filterString)
        {
            if (string.IsNullOrEmpty(filterString))
            {
                return new ValueRange<DateTime?>() { Start = null, End = null };
            }

            var partsOfString = filterString.Split(' ');

            if (partsOfString.Length == 3 && partsOfString[1] == "-")
            {
                return new ValueRange<DateTime?>()
                {
                    Start = DateTime.ParseExact(partsOfString[0], CommonConstants.DATE_RANGE_FILTER_FORMAT, null),
                    End   = DateTime.ParseExact(partsOfString[2], CommonConstants.DATE_RANGE_FILTER_FORMAT, null).GetWithMaxHour()
                };
            }
            else if (partsOfString.Length == 2 && partsOfString[0] == "<=")
            {
                return new ValueRange<DateTime?>()
                {
                    End = DateTime.ParseExact(partsOfString[1], CommonConstants.DATE_RANGE_FILTER_FORMAT, null).GetWithMaxHour()
                };
            }
            else if (partsOfString.Length == 2 && partsOfString[0] == ">=")
            {
                return new ValueRange<DateTime?>()
                {
                    Start = DateTime.ParseExact(partsOfString[1], CommonConstants.DATE_RANGE_FILTER_FORMAT, null),
                };
            }
            throw new ArgumentException("The syntax of expression is not valid.", nameof(filterString));
        }
    }
}
