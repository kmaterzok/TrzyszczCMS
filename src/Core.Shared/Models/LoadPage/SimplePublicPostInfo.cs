using System;

namespace TrzyszczCMS.Core.Shared.Models.LoadPage
{
    /// <summary>
    /// The simple information about a post that may be accessed publicly.
    /// Data dipslayed in post listings, etc.
    /// </summary>
    public class SimplePublicPostInfo
    {
        /// <summary>
        /// SEO friendly name of the page used in its URI.
        /// </summary>
        public string UriName { get; set; }
        /// <summary>
        /// The page's dipslayed title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Information about authors provided by the page creator.
        /// </summary>
        public string AuthorsInfo { get; set; }
        /// <summary>
        /// Description of the page.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Timestamp of the publishing the page.
        /// </summary>
        public DateTime PublishUtcTimestamp { get; set; }
    }
}
