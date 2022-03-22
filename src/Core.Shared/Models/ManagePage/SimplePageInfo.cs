using System;

namespace TrzyszczCMS.Core.Shared.Models.ManagePage
{
    /// <summary>
    /// The simple information about page that will be displayed.
    /// Used for grid listings.
    /// </summary>
    public class SimplePageInfo
    {
        /// <summary>
        /// Row ID of the page.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SEO friendly name of the page used in its URI.
        /// </summary>
        public string UriName { get; set; }
        /// <summary>
        /// The page's dipslayed title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Creation timestamp of the page.
        /// </summary>
        public DateTime CreateUtcTimestamp { get; set; }
        /// <summary>
        /// Timestamp of the publishing the page.
        /// </summary>
        public DateTime PublishUtcTimestamp { get; set; }

    }
}
