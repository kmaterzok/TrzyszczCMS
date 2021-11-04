using System;

namespace Core.Shared.Models.ManagePage
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
        /// The page's dipslayed title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Creation timestamp of the page.
        /// </summary>
        public DateTime CreateUtcTimestamp { get; set; }
    }
}
