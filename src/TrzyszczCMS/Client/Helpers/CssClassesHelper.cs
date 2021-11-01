namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// The class of helpers for handling CSS classes
    /// </summary>
    public static class CssClassesHelper
    {
        /// <summary>
        /// Get CSS classes string for disabled or enabled link. Suggested if you have a block filled with links.
        /// </summary>
        /// <param name="enabled">Is the link enabled</param>
        /// <returns>CSS classes string</returns>
        public static string ClassesForLink(bool enabled)
        {
            return enabled ? string.Empty : "disabled text-decoration-none cursor-default opacity-50";
        }
    }
}
