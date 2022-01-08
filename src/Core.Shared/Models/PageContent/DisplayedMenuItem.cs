using System.Collections.Generic;

namespace Core.Shared.Models.PageContent
{
    /// <summary>
    /// The most basic info of menu item used during presenting a page.
    /// </summary>
    public class DisplayedMenuItem
    {
        /// <summary>
        /// Displayed name of this item in a menu.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Uri that will be followed during a click.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Available sub item of this item
        /// </summary>
        public List<DisplayedMenuItem> SubItems { get; set; }
    }
}
