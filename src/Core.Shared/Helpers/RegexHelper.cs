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
            Regex.IsMatch(uriName ?? string.Empty, @"^[^!@#$%^&*()=+\[\]\{\};:/?\\]+$");

        /// <summary>
        /// Check if <paramref name="username"/> complies the rules of valid login / username.
        /// </summary>
        /// <param name="username">Checked username</param>
        /// <returns>Is username valid</returns>
        public static bool IsValidUserName(string username) =>
            Regex.IsMatch(username ?? string.Empty, @"^[a-z0-9_\-]+$");
    }
}
