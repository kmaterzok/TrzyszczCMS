using System.Text.RegularExpressions;

namespace TrzyszczCMS.Core.Application.Helpers
{
    /// <summary>
    /// Helper methods for sanitising and verifying content.
    /// </summary>
    public static class SanitiseHelper
    {
        /// <summary>
        /// Prepare <paramref name="text"/> for usage in a URI or its part.
        /// </summary>
        /// <param name="text">Unsanitised text</param>
        /// <returns>Sanitised text for usage in a URI or its part</returns>
        public static string GetStringReadyForUri(string text) =>
            Regex.Replace(text?.ToLower().Replace(' ', '-') ?? string.Empty, @"[^a-z0-9\-\ ]", string.Empty);
    }
}
