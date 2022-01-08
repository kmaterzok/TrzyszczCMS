using Core.Shared.Enums;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.PageContent.Modules
{
    /// <summary>
    /// The viewmodel for a heading banner page module.
    /// </summary>
    public class HeadingBannerModuleViewModel : ViewModelBase, IModuleViewModelBase
    {
        #region Properties
        public PageModuleType ModuleType { get; private set; }

        private string _backgroundPictureAccessGuid;
        /// <summary>
        /// GUID of the picture that will be a backgound for a banner.
        /// </summary>
        public string BackgroundPictureAccessGuid
        {
            get => this._backgroundPictureAccessGuid;
            set => Set(ref _backgroundPictureAccessGuid, value, nameof(BackgroundPictureAccessGuid));
        }
        #endregion

        #region Ctor
        public HeadingBannerModuleViewModel() =>
            this.ModuleType = PageModuleType.HeadingBanner;
        #endregion
    }
}
