namespace Core.Shared.Enums
{
    /// <summary>
    /// The type of module that displays content in a block within a page.
    /// </summary>
    public enum PageModuleType
    {
        /// <summary>
        /// Text formatted as HTML, displayed as continuous block of text.
        /// </summary>
        TextWall = 1,
        /// <summary>
        /// The fullscreen / height limited box containing picture,
        /// centered heading, some text and horizontal menu.
        /// </summary>
        HeadingBanner = 2
    }
}
