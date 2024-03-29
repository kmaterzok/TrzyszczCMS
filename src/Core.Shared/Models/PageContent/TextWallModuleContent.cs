﻿using TrzyszczCMS.Core.Shared.Enums;

namespace TrzyszczCMS.Core.Shared.Models.PageContent
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
        /// Main content section width.
        /// </summary>
        public TextWallSectionWidth SectionWidth { get; set; }
    }
}
