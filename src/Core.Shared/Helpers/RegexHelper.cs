using System.Text.RegularExpressions;

namespace Core.Shared.Helpers
{
    public static class RegexHelper
    {
        /// <summary>
        /// Check if <paramref name="uriName"/> complies the regular expression of a proper URI name of the page.
        /// </summary>
        /// <param name="uriName">Checked URI name</param>
        /// <returns>Is URI name valid</returns>
        public static bool IsValidPageUriName(string uriName) =>
            Regex.IsMatch(uriName, @"^[^!@#$%^&*()=+\[\]\{\};:/?\\]+$");
    }
}
