using System.Collections.Generic;
using System.Text;

namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// The class for easing handling of HTML markups and entities.
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Create an unordered list from HTML and enumeration of items for this list.
        /// </summary>
        /// <param name="listItems">Items for embedding in the list</param>
        /// <returns>Produced HTML code of the list</returns>
        public static StringBuilder MakeUnorderedList(IEnumerable<string> listItems)
        {
            var markuppedListring = new StringBuilder("<ul>");
            foreach(var listItem in listItems)
            {
                markuppedListring.Append("<li>")
                                 .Append(listItem)
                                 .Append("</li>");
            }
            return markuppedListring.Append("</ul>");
        }
    }
}
