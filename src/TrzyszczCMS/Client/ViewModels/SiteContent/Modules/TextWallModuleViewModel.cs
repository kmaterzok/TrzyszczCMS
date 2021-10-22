using Core.Shared.Enums;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.SiteContent.Modules
{
    /// <summary>
    /// The viewmodel for text wall page module.
    /// </summary>
    public class TextWallModuleViewModel : ViewModelBase, IModuleViewModelBase
    {
        #region Properties
        public PageModuleType ModuleType { get; private set; }

        private TextWallSectionWidth _sectionWidth;
        /// <summary>
        /// Max width of displayed text on the screen in pixels.
        /// </summary>
        public TextWallSectionWidth SectionWidth
        {
            get => _sectionWidth;
            set => Set(ref _sectionWidth, value, nameof(SectionWidth));
        }

        private string _displayedMarkDown;
        /// <summary>
        /// Main MarkDown text to display.
        /// </summary>
        public string SectionMarkDown
        {
            get => this._displayedMarkDown;
            set => Set(ref _displayedMarkDown, value, nameof(SectionMarkDown));
        }

        private string _leftAsideMarkDown;
        /// <summary>
        /// MarkDown text displayed on the left side of the main content.
        /// </summary>
        public string LeftAsideMarkDown
        {
            get => _leftAsideMarkDown;
            set => Set(ref _leftAsideMarkDown, value, nameof(LeftAsideMarkDown));
        }

        private string _rightAsideMarkDown;
        /// <summary>
        /// MarkDown text displayed on the right side of the main content.
        /// </summary>
        public string RightAsideMarkDown
        {
            get => _rightAsideMarkDown;
            set => Set(ref _rightAsideMarkDown, value, nameof(RightAsideMarkDown));
        }
        #endregion

        #region Ctor
        public TextWallModuleViewModel()
        {
            this.ModuleType = PageModuleType.TextWall;
        }
        #endregion
    }
}
