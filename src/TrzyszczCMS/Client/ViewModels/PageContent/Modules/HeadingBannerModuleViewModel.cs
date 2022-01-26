using Core.Shared.Enums;
using Core.Shared.Models.PageContent;
using System.Text;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.PageContent.Modules
{
    /// <summary>
    /// The viewmodel for a heading banner page module.
    /// </summary>
    public class HeadingBannerModuleViewModel : ViewModelBase, IModuleViewModel
    {
        #region Properties
        public PageModuleType ModuleType { get; private set; }

        private HeadingBannerModuleContent _ModuleContent;
        /// <summary>
        /// Content of the module which the viewmodel gets data from.
        /// </summary>
        public HeadingBannerModuleContent ModuleContent
        {
            get => _ModuleContent;
            set => Set(ref _ModuleContent, value, nameof(ModuleContent));
        }
        /// <summary>
        /// CSS style of the banner
        /// </summary>
        public string BannerCssStyle
        {
            get
            {
                var cssStyle = new StringBuilder
                (
                    string.IsNullOrEmpty(this.ModuleContent.BackgroundPictureAccessGuid) ?
                        CssStyleHelper.GetDefaultTextColourCssStyle(!this.ModuleContent.DarkDescription) :
                        CssStyleHelper.GetBackgroundImageCssStyle(this.ModuleContent.BackgroundPictureAccessGuid)
                );
                cssStyle.Append(" height: ")
                        .Append(this.ModuleContent.ViewportHeight.GetBannerHeightCssValue())
                        .Append("; min-height: 200px; ");

                cssStyle.Append(CssStyleHelper.GetDefaultTextColourCssStyle(this.ModuleContent.DarkDescription));

                return cssStyle.ToString();
            }
        }
        /// <summary>
        /// Text shadow colour in the banner in the bottom.
        /// </summary>
        public string ShadowColor => this.ModuleContent.DarkDescription ? "white" : "black";
        #endregion

        #region Ctor
        public HeadingBannerModuleViewModel(HeadingBannerModuleContent moduleContent)
        {
            this.ModuleContent = moduleContent;
            this.ModuleType = PageModuleType.HeadingBanner;
        }
        #endregion
    }
}
