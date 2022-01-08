using Core.Application.Enums;
using Core.Application.Helpers;
using Core.Application.Models.Deposits;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models.PageContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class EditedHeadingBannerModuleViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// Reference on an object containing edited data.
        /// </summary>
        private HeadingBannerModuleContent _headingBannerModuleContent;
        /// <summary>
        /// Used e.g. for validating file access.
        /// </summary>
        private readonly IManageFileService _manageFileService;
        #endregion

        #region Properties :: General
        /// <summary>
        /// Fired when all data is proper and a user wants to exit heading banner editor.
        /// </summary>
        public Action OnExitingEditor { get; set; }

        /// <summary>
        /// Fired when any displayed data are modified.
        /// </summary>
        public EventHandler OnDataSet { get; set; }

        /// <summary>
        /// Is description displayed on the banner.
        /// </summary>
        public bool DisplayDescription
        {
            get => this._headingBannerModuleContent.DisplayDescription;
            set
            {
                this._headingBannerModuleContent.DisplayDescription = value;
                PropertyValueChanged(nameof(DisplayDescription));
            }
        }
        /// <summary>
        /// Is authors info displayed on the banner.
        /// </summary>
        public bool DisplayAuthorsInfo
        {
            get => this._headingBannerModuleContent.DisplayAuthorsInfo;
            set
            {
                this._headingBannerModuleContent.DisplayAuthorsInfo = value;
                PropertyValueChanged(nameof(DisplayAuthorsInfo));
            }
        }
        /// <summary>
        /// If says if the provided text elements are displayed with a dark colour.
        /// </summary>
        public bool DarkDescription
        {
            get => this._headingBannerModuleContent.DarkDescription;
            set
            {
                this._headingBannerModuleContent.DarkDescription = value;
                PropertyValueChanged(nameof(DarkDescription));
            }
        }
        /// <summary>
        /// Height of the displayed element on the page
        /// </summary>
        public HeadingBannerHeight ViewportHeight
        {
            get => this._headingBannerModuleContent.ViewportHeight;
            set
            {
                this._headingBannerModuleContent.ViewportHeight = value;
                PropertyValueChanged(nameof(ViewportHeight));
            }
        }
        /// <summary>
        /// Access Id (GUID) of a background graphics from the storage.
        /// </summary>
        public string BackgroundPictureAccessGuid
        {
            get => this._headingBannerModuleContent.BackgroundPictureAccessGuid;
            set
            {
                this._headingBannerModuleContent.BackgroundPictureAccessGuid = value;
                PropertyValueChanged(nameof(BackgroundPictureAccessGuid));
            }
        }
        /// <summary>
        /// Attach and dipslay main link menu atop of the heading banner.
        /// </summary>
        public bool AttachLinkMenu
        {
            get => this._headingBannerModuleContent.AttachLinkMenu;
            set
            {
                this._headingBannerModuleContent.AttachLinkMenu = value;
                PropertyValueChanged(nameof(AttachLinkMenu));
            }
        }

        private string _backgroundPictureAccessGuidValidationMessage;
        /// <summary>
        /// Access Id (GUID) of a background graphics from the storage.
        /// </summary>
        public string BackgroundPictureAccessGuidValidationMessage
        {
            get => _backgroundPictureAccessGuidValidationMessage;
            set => Set(ref _backgroundPictureAccessGuidValidationMessage, value, nameof(BackgroundPictureAccessGuidValidationMessage));
        }
        #endregion


        #region Ctor
        public EditedHeadingBannerModuleViewModel(HeadingBannerModuleContent headingBannerModuleContent, IManageFileService manageFileService)
        {
            this._headingBannerModuleContent = headingBannerModuleContent;
            this._manageFileService = manageFileService;
        }
        #endregion


        #region Methods
        public async Task ValidateAndExitAsync()
        {
            if (await this.ValidateAndInformAsync())
            {
                this.OnExitingEditor?.Invoke();
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Notify UI thread about change of the property
        /// and invoke action responsible for saving changes periodically.
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        private void PropertyValueChanged(string propertyName)
        {
            NotifyPropertyChanged(propertyName);
            OnDataSet?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Check validity of the entered data and inform a user
        /// by setting proper messages.
        /// </summary>
        /// <returns>Task returning if data is valid</returns>
        private async Task<bool> ValidateAndInformAsync()
        {
            this.ClearReachableMessages();
            bool valid = true;

            // *** BackgroundPictureAccessGuid ***
            if (!string.IsNullOrEmpty(this.BackgroundPictureAccessGuid))
            {
                if (!Guid.TryParse(this.BackgroundPictureAccessGuid, out _))
                {
                    this.BackgroundPictureAccessGuidValidationMessage = "Bad ID format";
                    valid = false;
                }
                else
                {
                    var verdict = await this._manageFileService.FileIsGraphics(this.BackgroundPictureAccessGuid);
                    switch (verdict)
                    {
                        case FileTypeCheckResult.NotFound:
                            this.BackgroundPictureAccessGuidValidationMessage = "File not found";
                            break;
                        case FileTypeCheckResult.NotApplicableMimeType:
                            this.BackgroundPictureAccessGuidValidationMessage = "File is not a graphics.";
                            break;
                        case FileTypeCheckResult.OK:
                            break;
                        default:
                            throw ExceptionMaker.Member.Unsupported(verdict, nameof(verdict));
                    }
                    if (verdict != FileTypeCheckResult.OK)
                    {
                        valid = false;
                    }
                }
            }
            // *** ***
            return valid;
        }
        /// <summary>
        /// Clear all the messages that were set programatically within this class.
        /// </summary>
        private void ClearReachableMessages() =>
            this.BackgroundPictureAccessGuidValidationMessage = string.Empty;
        #endregion
    }
}
