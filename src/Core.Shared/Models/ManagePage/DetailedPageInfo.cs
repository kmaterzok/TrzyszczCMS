using Core.Shared.Enums;
using Core.Shared.Models.PageContent;
using System;
using System.Collections.Generic;

namespace Core.Shared.Models.ManagePage
{
    /// <summary>
    /// All necessary info for managing and editing
    /// </summary>
    public class DetailedPageInfo
    {
        /// <summary>
        /// Database row ID of the page
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The page's dipslayed title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// SEO friendly name of the page used in its URI.
        /// </summary>
        public string UriName { get; set; }
        /// <summary>
        /// Timestamp of the publishing the page
        /// </summary>
        public DateTime PublishUtcTimestamp { get; set; }
        /// <summary>
        /// Type of the page the information applies to.
        /// </summary>
        public PageType PageType { get; set; }
        /// <summary>
        /// All page's modules delivering content
        /// </summary>
        public List<ModuleContent> ModuleContents { get; set; }



        /// <summary>
        /// Create a new empty instance of details for creating a new page.
        /// </summary>
        /// <param name="type">Desired page type</param>
        /// <returns>Empty instance</returns>
        public static DetailedPageInfo MakeEmpty(PageType type) => new DetailedPageInfo()
        {
            Id = 0,
            ModuleContents = new List<ModuleContent>(),
            PageType = type,
            PublishUtcTimestamp = DateTime.UtcNow,
            Title = (string)null,
            UriName = (string)null
        };
    }
}
