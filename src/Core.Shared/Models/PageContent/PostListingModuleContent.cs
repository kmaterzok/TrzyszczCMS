using TrzyszczCMS.Core.Shared.Enums;

namespace TrzyszczCMS.Core.Shared.Models.PageContent
{
    /// <summary>
    /// The class of post listing's content without posts information.
    /// </summary>
    public class PostListingModuleContent
    {
        /// <summary>
        /// Max width of the dipslayed content in the module.
        /// </summary>
        public PostListingWidth Width { get; set; }
    }
}
