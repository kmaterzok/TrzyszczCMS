using Core.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Core.Shared.Models.PageContent
{
    /// <summary>
    /// The class of a heading banner's content and properties.
    /// Used for management and page display.
    /// </summary>
    public class HeadingBannerModuleContent
    {
        /// <summary>
        /// Displayed description of the page.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Is description displayed on the banner.
        /// </summary>
        public bool DisplayDescription { get; set; }
        /// <summary>
        /// Displayed authors info of the page
        /// </summary>
        public string AuthorsInfo { get; set; }
        /// <summary>
        /// Is authors info displayed on the banner.
        /// </summary>
        public bool DisplayAuthorsInfo { get; set; }
        /// <summary>
        /// If says if the provided text elements are displayed with a dark colour.
        /// </summary>
        public bool DarkDescription { get; set; }
        /// <summary>
        /// Height of the displayed element on the page
        /// </summary>
        public HeadingBannerHeight ViewportHeight { get; set; }
        /// <summary>
        /// Displayed elements of the menu that is placed atop of the heading banner.
        /// </summary>
        public List<DisplayedMenuItem> MenuItems { get; set; }
        /// <summary>
        /// Access Id (GUID) of a background graphics from the storage.
        /// </summary>
        public string BackgroundPictureAccessGuid { get; set; }
        /// <summary>
        /// Attach and dipslay main link menu atop of the heading banner.
        /// </summary>
        public bool AttachLinkMenu { get; set; }
        /// <summary>
        /// Title of the page that contains a module.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Timestamp of the publishing the page.
        /// </summary>
        public DateTime PublishUtcTimestamp { get; set; }
    }
}
