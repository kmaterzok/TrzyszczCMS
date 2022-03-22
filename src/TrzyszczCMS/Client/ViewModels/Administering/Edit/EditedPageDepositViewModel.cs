using TrzyszczCMS.Core.Application.Enums;
using TrzyszczCMS.Core.Application.Models.Deposits;
using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models.ManagePage;
using TrzyszczCMS.Core.Shared.Models.PageContent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.Helpers.Extensions;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    /// <summary>
    /// ViewModel complementary for <see cref="EditedPageDeposit"/> class.
    /// It provides sufficient data notifications and mapping.
    /// </summary>
    public class EditedPageDepositViewModel : ViewModelBase
    {
        #region Fields
        private readonly IManagePageService _managePageService;
        private string _oldUriName;
        #endregion

        #region Properties :: Work & modes
        private DataEditorMode _pageEditorMode;
        /// <summary>
        /// Work mode of editor
        /// </summary>
        public DataEditorMode PageEditorMode
        {
            get => _pageEditorMode;
            set => Set(ref _pageEditorMode, value, nameof(PageEditorMode));
        }
        private PageManagementTool _currentManagementTool;
        /// <summary>
        /// Currently used and displayed management tool for the edited / created page.
        /// </summary>
        public PageManagementTool CurrentManagementTool
        {
            get => _currentManagementTool;
            set => Set(ref _currentManagementTool, value, nameof(CurrentManagementTool));
        }
        /// <summary>
        /// Fired when any displayed data are modified.
        /// </summary>
        public EventHandler OnDataSet { get; set; }
        #endregion

        #region Properties :: Validation messages
        private string _uriNameValidationMessage;
        /// <summary>
        /// The additional message about URI name validation.
        /// </summary>
        public string UriNameValidationMessage
        {
            get => _uriNameValidationMessage;
            set => Set(ref _uriNameValidationMessage, value, nameof(UriNameValidationMessage));
        }

        private string _titleValidationMessage;
        /// <summary>
        /// The additional message about title validation.
        /// </summary>
        public string TitleValidationMessage
        {
            get => _titleValidationMessage;
            set => Set(ref _titleValidationMessage, value, nameof(TitleValidationMessage));
        }

        private string _publishUtcTimestampTimeValidationMessage;
        /// <summary>
        /// The additional message about publication time validation.
        /// </summary>
        public string PublishUtcTimestampTimeValidationMessage
        {
            get => _publishUtcTimestampTimeValidationMessage;
            set => Set(ref _publishUtcTimestampTimeValidationMessage, value, nameof(PublishUtcTimestampTimeValidationMessage));
        }

        private string _publishUtcTimestampDateValidationMessage;
        /// <summary>
        /// The additional message about publication date validation.
        /// </summary>
        public string PublishUtcTimestampDateValidationMessage
        {
            get => _publishUtcTimestampDateValidationMessage;
            set => Set(ref _publishUtcTimestampDateValidationMessage, value, nameof(PublishUtcTimestampDateValidationMessage));
        }
        #endregion

        #region Properties :: Edited data
        /// <summary>
        /// Database row ID of the page
        /// </summary>
        public int IdOfPage { get; set; }
        /// <summary>
        /// Type of the page the information applies to.
        /// </summary>
        public PageType PageType { get; set; }
        
        private string _title;
        /// <summary>
        /// The page's dipslayed title
        /// </summary>
        public string Title
        {
            get => _title;
            set { Set(ref _title, value, nameof(Title)); this.OnDataSet?.Invoke(this, EventArgs.Empty); }
        }
        
        private string _uriName;
        /// <summary>
        /// SEO friendly name of the page used in its URI.
        /// </summary>
        public string UriName
        {
            get => _uriName;
            set { Set(ref _uriName, value, nameof(UriName)); this.OnDataSet?.Invoke(this, EventArgs.Empty); }
        }

        private DateTime _publishUtcTimestampTime;
        /// <summary>
        /// Timestamp's time of the publishing the page
        /// </summary>
        public DateTime PublishUtcTimestampTime
        {
            get => _publishUtcTimestampTime;
            set { Set(ref _publishUtcTimestampTime, value, nameof(PublishUtcTimestampTime)); this.OnDataSet?.Invoke(this, EventArgs.Empty); }
        }

        private DateTime? _publishUtcTimestampDate;
        /// <summary>
        /// Timestamp's date of the publishing the page
        /// </summary>
        public DateTime? PublishUtcTimestampDate
        {
            get => _publishUtcTimestampDate;
            set { Set(ref _publishUtcTimestampDate, value, nameof(PublishUtcTimestampDate)); this.OnDataSet?.Invoke(this, EventArgs.Empty); }
        }

        private string _authorsInfo;
        /// <summary>
        /// Information about authors provided by the page creator.
        /// </summary>
        public string AuthorsInfo
        {
            get => _authorsInfo;
            set { Set(ref _authorsInfo, value, nameof(AuthorsInfo)); this.OnDataSet?.Invoke(this, EventArgs.Empty); }
        }

        private string _description;
        /// <summary>
        /// Description of the page.
        /// </summary>
        public string Description
        {
            get => _description;
            set { Set(ref _description, value, nameof(Description)); this.OnDataSet?.Invoke(this, EventArgs.Empty); }
        }

        private List<GridItem<ModuleContent>> _moduleContents;
        /// <summary>
        /// Modules displayed on the page with their content.
        /// </summary>
        public List<GridItem<ModuleContent>> ModuleContents
        {
            get => _moduleContents;
            set => Set(ref _moduleContents, value, nameof(ModuleContents));
        }
        /// <summary>
        /// The index of the module that is being edited.
        /// </summary>
        public int EditedModuleListIndex { get; set; }
        #endregion

        #region Ctor
        public EditedPageDepositViewModel(EditedPageDeposit deposit, IManagePageService managePageService)
        {
            this._managePageService      = managePageService;

            this.PageEditorMode          = deposit.PageEditorMode;
            this.CurrentManagementTool   = deposit.CurrentManagementTool;
            this.EditedModuleListIndex   = deposit.EditedModuleListIndex;
            this._oldUriName             = deposit.OldUriName;

            this.IdOfPage                = deposit.PageDetails.Id;
            this.Title                   = deposit.PageDetails.Title;
            this.UriName                 = deposit.PageDetails.UriName;
            this.PublishUtcTimestampTime = deposit.PageDetails.PublishUtcTimestamp;
            this.PublishUtcTimestampDate = deposit.PageDetails.PublishUtcTimestamp;
            this.PageType                = deposit.PageDetails.PageType;
            this.ModuleContents          = deposit.PageDetails.ModuleContents.ToGridItemList();
            this.AuthorsInfo             = deposit.PageDetails.AuthorsInfo;
            this.Description             = deposit.PageDetails.Description;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create an instance of <see cref="EditedPageDeposit"/> based on this object's data.
        /// </summary>
        /// <returns>Mapped object</returns>
        public EditedPageDeposit ToDeposit() => new EditedPageDeposit()
        {
            CurrentManagementTool   = this.CurrentManagementTool,
            PageEditorMode          = this.PageEditorMode,
            EditedModuleListIndex   = this.EditedModuleListIndex,
            OldUriName              = this._oldUriName,
            PageDetails             = this.GetDetailedPageInfo()
        };

        /// <summary>
        /// Create and get a detailed page info.
        /// </summary>
        /// <returns>Modifiable and manageable information about page</returns>
        public DetailedPageInfo GetDetailedPageInfo() => new DetailedPageInfo()
        {
            Id                  = this.IdOfPage,
            ModuleContents      = this.ModuleContents.ToOrdinaryList(),
            PageType            = this.PageType,
            PublishUtcTimestamp = this.PublishUtcTimestampDate.Value.TruncateHMS().Add(this.PublishUtcTimestampTime.GetHMS()),
            Title               = this.Title,
            UriName             = this.UriName,
            AuthorsInfo         = this.AuthorsInfo,
            Description         = this.Description
        };

        /// <summary>
        /// Check validity of the data.
        /// </summary>
        /// <returns>Is data valid</returns>
        public async Task<bool> ValidateAndInformAsync()
        {
            this.ClearReachableMessages();
            bool valid = true;

            // *** UriName ***
            bool noChangeInName = this._oldUriName == this.UriName;
            this.UriNameValidationMessage = ValidationApplier.CheckRequired(this.UriName, ref valid);
            if (noChangeInName || !valid)
            {
                // Do nothing
            }
            else if ((await this._managePageService.PageUriNameExists(this.UriName)).GetValue(out Tuple<bool> success, out string error))
            {
                if (success.Item1)
                {
                    this.UriNameValidationMessage = "Already in use. Change it.";
                }
                valid = !success.Item1;
            }
            else
            {
                this.UriNameValidationMessage = error == "PatternMismatch" ?
                    "Forbidden characters. Change it." :
                    "Incorrect data.";
                
                valid = false;
            }
            // *** ***

            // *** Required fields ***
            this.TitleValidationMessage                   = ValidationApplier.CheckRequired(this.Title, ref valid);
            this.PublishUtcTimestampDateValidationMessage = ValidationApplier.CheckRequired(this.PublishUtcTimestampDate, ref valid);
            this.PublishUtcTimestampTimeValidationMessage = ValidationApplier.CheckRequired(this.PublishUtcTimestampTime, ref valid);
            // *** ***

            return valid;
        }
        /// <summary>
        /// Move module content item up or down.
        /// </summary>
        /// <param name="menuItem">Moved item</param>
        /// <param name="moveUp"><c>true</c> - up, <c>false</c> - down</param>
        public void MoveModuleItem(GridItem<ModuleContent> menuItem, bool moveUp)
        {
            this.ModuleContents.MoveItem(menuItem, moveUp);
            this.NotifyPropertyChanged(nameof(this.ModuleContents));
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Clear all the messages that were set programatically within this class.
        /// </summary>
        private void ClearReachableMessages()
        {
            this.UriNameValidationMessage                 = string.Empty;
            this.TitleValidationMessage                   = string.Empty;
            this.PublishUtcTimestampDateValidationMessage = string.Empty;
            this.PublishUtcTimestampTimeValidationMessage = string.Empty;
        }
        #endregion

    }
}
