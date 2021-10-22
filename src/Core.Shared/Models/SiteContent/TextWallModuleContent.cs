using Core.Shared.Enums;

namespace Core.Shared.Models.SiteContent
{
    /// <summary>
    /// The class of module's content.
    /// </summary>
    public class TextWallModuleContent
    {
        /// <summary>
        /// The content for the left aside block.
        /// </summary>
        public string LeftAsideMarkDownContent { get; set; }
        /// <summary>
        /// The content for the main block displaying crucial text.
        /// </summary>
        public string RightAsideMarkDownContent { get; set; }
        /// <summary>
        /// The content for the right aside block.
        /// </summary>
        public string SectionMarkDownContent { get; set; }
        /// <summary>
        /// Main contect section width.
        /// </summary>
        public TextWallSectionWidth SectionWidth { get; set; }
    }
}
