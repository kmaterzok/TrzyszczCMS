using Core.Application.Enums;
using Core.Application.Models.Deposits;
using Core.Shared.Enums;
using Core.Shared.Models.ManagePage;
using Core.Shared.Models.PageContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    /// <summary>
    /// ViewModel complementary for <see cref="EditedPageDeposit"/> class.
    /// It provides sufficient data notifications and mapping.
    /// </summary>
    public class EditedPageDepositViewModel : ViewModelBase
    {
        #region Properties :: Work & modes
        private PageEditorMode _pageEditorMode;
        /// <summary>
        /// Work mode of editor
        /// </summary>
        public PageEditorMode PageEditorMode
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
            set => Set(ref _title, value, nameof(Title));
        }
        /// <summary>
        /// SEO friendly name of the page used in its URI.
        /// </summary>
        private string _uriName;
        public string UriName
        {
            get => _uriName;
            set => Set(ref _uriName, value, nameof(UriName));
        }
        /// <summary>
        /// Timestamp of the publishing the page
        /// </summary>
        private DateTime _publishUtcTimestamp;
        public DateTime PublishUtcTimestamp
        {
            get => _publishUtcTimestamp;
            set => Set(ref _publishUtcTimestamp, value, nameof(PublishUtcTimestamp));
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
        ///// <summary>
        ///// Modify <see cref="ModuleContents"/> and notify the change.
        ///// </summary>
        ///// <param name="modifier">Action for modifying the <see cref="ModuleContents"/> </param>
        //public void ModifyModuleContents(Action<List<GridItem<ModuleContent>>> modifier) =>
        //    Set(() => modifier.Invoke(_moduleContents), nameof(ModuleContents));
        #endregion

        #region Ctor
        public EditedPageDepositViewModel(EditedPageDeposit deposit)
        {
            this.PageEditorMode        = deposit.PageEditorMode;
            this.CurrentManagementTool = deposit.CurrentManagementTool;
            this.EditedModuleListIndex = deposit.EditedModuleListIndex;

            this.IdOfPage              = deposit.PageDetails.Id;
            this.Title                 = deposit.PageDetails.Title;
            this.UriName               = deposit.PageDetails.UriName;
            this.PublishUtcTimestamp   = deposit.PageDetails.PublishUtcTimestamp;
            this.PageType              = deposit.PageDetails.PageType;
            this.ModuleContents        = deposit.PageDetails.ModuleContents.ToGridItemList();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create an instance of <see cref="EditedPageDeposit"/> based on this object's data.
        /// </summary>
        /// <returns>Mapped object</returns>
        public EditedPageDeposit ToDeposit() => new EditedPageDeposit()
        {
            CurrentManagementTool = this.CurrentManagementTool,
            PageEditorMode        = this.PageEditorMode,
            EditedModuleListIndex = this.EditedModuleListIndex,

            PageDetails           = new DetailedPageInfo()
            {
                Id                  = this.IdOfPage,
                ModuleContents      = this.ModuleContents.ToOrdinaryList(),
                PageType            = this.PageType,
                PublishUtcTimestamp = this.PublishUtcTimestamp,
                Title               = this.Title,
                UriName             = this.UriName
            }
        };
        #endregion
    }
}
