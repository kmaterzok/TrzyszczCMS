namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// The class of helpers for handling CSS styles.
    /// </summary>
    public static class CssStyleHelper
    {
        /// <summary>
        /// Get a style for backgound image form the backend.
        /// </summary>
        /// <param name="fileAccessGuid">GUID of the picture</param>
        /// <returns>CSS style for a background</returns>
        public static string GetBackgroundImageCssStyle(string fileAccessGuid) =>
            string.IsNullOrEmpty(fileAccessGuid) ?
                string.Empty :
                $"background-image: url('/Storage/GetFile/{fileAccessGuid}');";

        /// <summary>
        /// Get a style for a default colour of text.
        /// </summary>
        /// <param name="darkColour">Is the colur dark</param>
        /// <returns>CSS style for a text colour</returns>
        public static string GetDefaultTextColourCssStyle(bool darkColour) =>
            darkColour ? $"color: #111;" : $"color: #eee;";

        /// <summary>
        /// Get a style for a default colour of background.
        /// </summary>
        /// <param name="darkColour">Is the colur dark</param>
        /// <returns>CSS style for a background colour</returns>
        public static string GetDefaultBackgroundColourCssStyle(bool darkColour) =>
            darkColour ? $"background-color: #111;" : $"background-color: #eee;";

        /// <summary>
        /// Get a style for buttons and selects present in the heading banner menu.
        /// </summary>
        /// <param name="text">Text hold in the buttons or select's disabled option</param>
        /// <returns>Style for element</returns>
        public static string CssMenuItemWidth(string text) =>
            text.Length > 14 ? "width: 250px;" : "width: 150px;";
    }
}
