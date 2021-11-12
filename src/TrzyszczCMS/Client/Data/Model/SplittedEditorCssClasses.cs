namespace TrzyszczCMS.Client.Data.Model
{
    /// <summary>
    /// CSS classes for editors of content with previewing changes alongside.
    /// </summary>
    public class SplittedEditorCssClasses
    {
        /// <summary>
        /// CSS classes for container dedicated for editing content.
        /// </summary>
        public string EditorCss { get; set; }
        /// <summary>
        /// CSS classes for container dedicated for previewing changes.
        /// </summary>
        public string PreviewCss { get; set; }
    }
}
