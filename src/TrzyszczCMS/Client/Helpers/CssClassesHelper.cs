using Core.Shared.Helpers;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Data.Model;

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
        public static string ClassesForLink(bool enabled) =>
            enabled ? string.Empty : "disabled text-decoration-none cursor-default opacity-50";
        
        /// <summary>
        /// Get CSS classes for MarkDown editor
        /// </summary>
        /// <param name="mode">Mode of work of this editor</param>
        /// <returns>CSS classes set for editor and changes preview</returns>
        public static SplittedEditorCssClasses ClassesForMarkDownEditor(ToggledMarkDownViewMode mode) => mode switch
        {
            ToggledMarkDownViewMode.EditorWithPreview => new SplittedEditorCssClasses()
            {
                EditorCss = "col-6 pe-3",
                PreviewCss = "col-6 ps-3"
            },
            ToggledMarkDownViewMode.Editor => new SplittedEditorCssClasses()
            {
                EditorCss = "col-12 px-0",
                PreviewCss = "collapse"
            },
            ToggledMarkDownViewMode.Preview => new SplittedEditorCssClasses()
            {
                EditorCss = "collapse",
                PreviewCss = "col-12 px-0"
            },
            _ => throw ExceptionMaker.Argument.Unsupported(mode, nameof(mode))
        };
        
        /// <summary>
        /// Get CSS class for collapsing an item
        /// </summary>
        /// <param name="enabled">Is the object visible</param>
        /// <returns>CSS class for collapsing the item</returns>
        public static string ClassCollapsingElement(bool enabled) =>
            enabled ? string.Empty : "collapse";
    }
}
