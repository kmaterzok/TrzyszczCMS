using System;

namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// The class helping to apply validation messages.
    /// </summary>
    public static class ValidationApplier
    {
        /// <summary>
        /// Check if <paramref name="value"/> is not empty as it is required one.
        /// </summary>
        /// <param name="value">checked string</param>
        /// <param name="valid">Is the checked value valid and has a value</param>
        /// <returns>Set <paramref name="value"/> is OK</returns>
        public static string CheckRequired(string value, ref bool valid)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                valid = false;
                return "Required";
            }
            return string.Empty;
        }

        /// <summary>
        /// Check if <paramref name="value"/> is not empty as it is required one.
        /// </summary>
        /// <param name="value">checked value</param>
        /// <param name="valid">Is the checked value valid and has a value</param>
        /// <returns>Set <paramref name="value"/> is OK</returns>
        public static string CheckRequired(DateTime? value, ref bool valid)
        {
            if (!value.HasValue)
            {
                valid = false;
                return "Required";
            }
            return string.Empty;
        }
    }
}
