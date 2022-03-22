namespace TrzyszczCMS.Core.Server.Helpers
{
    /// <summary>
    /// The helper for handling MIME-oriented issues.
    /// </summary>
    public static class MimeTypeHelper
    {
        /// <summary>
        /// Check if the <paramref name="mimeType"/> MIME type applies to a graphics file.
        /// </summary>
        /// <param name="mimeType">Checked MIME type</param>
        /// <returns>The checked MIME type applies to graphics.</returns>
        public static bool IsGraphics(string mimeType) => mimeType.StartsWith("image/");
    }
}
