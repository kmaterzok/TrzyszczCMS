using Core.Application.Enums;
using Core.Application.Helpers;
using Core.Application.Models.Deposits;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Helpers.Extensions;
using Core.Shared.Models.PageContent;
using System;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class PageEditorViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private readonly IDataDepository _depository;
        private readonly IManagePageService _managePageService;
        private readonly IManageFileService _manageFileService;
        private readonly IAuthService _authService;
        private DelayedInvoker _delayedDepositoryUpdateInvoker;
        #endregion


        #region Properties :: General
        /// <summary>
        /// Fired when there is a need to exit the current view.
        /// </summary>
        public Action OnExitingView { get; set; }

        private bool _isManagementInfoProvided;
        /// <summary>
        /// It says whether there is data allowing to manage a page.
        /// </summary>
        public bool IsManagementInfoProvided
        {
            get => _isManagementInfoProvided;
            set => Set(ref _isManagementInfoProvided, value, nameof(IsManagementInfoProvided));
        }

        private bool _arePermissionsForSpecifiedManagementProvided;
        /// <summary>
        /// It says whether the signed in user has sufficient policies
        /// assigned to itself so it is able to manage page data.
        /// </summary>
        public bool ArePermissionsForSpecifiedManagementProvided
        {
            get => _arePermissionsForSpecifiedManagementProvided;
            set => Set(ref _arePermissionsForSpecifiedManagementProvided, value, nameof(ArePermissionsForSpecifiedManagementProvided));
        }

        private EditedPageDepositViewModel _editedPageDepositVM;
        /// <summary>
        /// Remapped instance of <see cref="EditedPageDepositVM"/> for editing purposes.
        /// </summary>
        public EditedPageDepositViewModel EditedPageDepositVM
        {
            get => _editedPageDepositVM;
            set => Set(ref _editedPageDepositVM, value, nameof(EditedPageDepositVM));
        }

        private EditedHeadingBannerModuleViewModel _currentlyEditedHeadingBannerVM;
        /// <summary>
        /// The viewmodel accessing the currently edited heading banner module data.
        /// </summary>
        public  EditedHeadingBannerModuleViewModel CurrentlyEditedHeadingBannerVM
        {
            get
            {
                if (_currentlyEditedHeadingBannerVM == null)
                {
                    var assignedModule = this.EditedPageDepositVM.ModuleContents[this.EditedPageDepositVM.EditedModuleListIndex]
                                                                 .Data.HeadingBannerModuleContent;
                    _currentlyEditedHeadingBannerVM = new EditedHeadingBannerModuleViewModel(assignedModule, this._manageFileService);
                    _currentlyEditedHeadingBannerVM.PropertyChanged += (_, e) => this.NotifyPropertyChanged(e.PropertyName);
                    _currentlyEditedHeadingBannerVM.OnDataSet += (_, _e) => this._delayedDepositoryUpdateInvoker.DelayedInvoke();
                    _currentlyEditedHeadingBannerVM.OnExitingEditor += BackToLayoutComposer;
                }
                return _currentlyEditedHeadingBannerVM;
            }
        }

        private EditedPostListingModuleViewModel _currentlyEditedPostListingVM;
        /// <summary>
        /// The viewmodel accessing the currently edited post listing module data.
        /// </summary>
        public EditedPostListingModuleViewModel CurrentlyEditedPostListingVM
        {
            get
            {
                if (_currentlyEditedPostListingVM == null)
                {
                    var assignedModule = this.EditedPageDepositVM.ModuleContents[this.EditedPageDepositVM.EditedModuleListIndex]
                                                                 .Data.PostListingModuleContent;
                    _currentlyEditedPostListingVM = new EditedPostListingModuleViewModel(assignedModule);
                    _currentlyEditedPostListingVM.PropertyChanged += (_, e) => this.NotifyPropertyChanged(e.PropertyName);
                    _currentlyEditedPostListingVM.OnDataSet += (_, _e) => this._delayedDepositoryUpdateInvoker.DelayedInvoke();
                    _currentlyEditedPostListingVM.OnExitingEditor += BackToLayoutComposer;
                }
                return _currentlyEditedPostListingVM;
            }
        }
        #endregion


        #region Properties :: Text wall editor
        /// <summary>
        /// The MarkDown editor's edited code.
        /// </summary>
        public string CurrentlyEditedMarkDownCode
        {
            get
            {
                var editedModuleListIndex = this.EditedPageDepositVM.EditedModuleListIndex;
                return this.EditedPageDepositVM.CurrentManagementTool switch
                {
                    PageManagementTool.PageLayoutComposer       => string.Empty,
                    PageManagementTool.TextWallSectionEditor    => this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.SectionMarkDownContent,
                    PageManagementTool.TextWallLeftAsideEditor  => this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.LeftAsideMarkDownContent,
                    PageManagementTool.TextWallRightAsideEditor => this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.RightAsideMarkDownContent,
                    _ => throw ExceptionMaker.Member.Invalid(this.EditedPageDepositVM.CurrentManagementTool, nameof(this.EditedPageDepositVM.CurrentManagementTool)),
                };
            }
            set
            {
                var editedModuleListIndex = this.EditedPageDepositVM.EditedModuleListIndex;
                switch (this.EditedPageDepositVM.CurrentManagementTool)
                {
                    case PageManagementTool.PageLayoutComposer:
                        break;
                    case PageManagementTool.TextWallSectionEditor:
                        this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.SectionMarkDownContent = value;
                        break;
                    case PageManagementTool.TextWallLeftAsideEditor:
                        this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.LeftAsideMarkDownContent = value;
                        break;
                    case PageManagementTool.TextWallRightAsideEditor:
                        this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.RightAsideMarkDownContent = value;
                        break;
                    default:
                        throw ExceptionMaker.Member.Invalid(this.EditedPageDepositVM.CurrentManagementTool, nameof(this.EditedPageDepositVM.CurrentManagementTool));
                }
            }
        }
        /// <summary>
        /// The MarkDown's editor width for the section of a page.
        /// </summary>
        public TextWallSectionWidth? CurrentlyEditedSectionWidth
        {
            get
            {
                var editedModuleListIndex = this.EditedPageDepositVM.EditedModuleListIndex;
                return this.EditedPageDepositVM.CurrentManagementTool switch
                {
                    PageManagementTool.PageLayoutComposer
                        or PageManagementTool.TextWallLeftAsideEditor
                        or PageManagementTool.TextWallRightAsideEditor => null,
                    PageManagementTool.TextWallSectionEditor           => this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.SectionWidth,

                    _ => throw ExceptionMaker.Member.Invalid(this.EditedPageDepositVM.CurrentManagementTool, nameof(this.EditedPageDepositVM.CurrentManagementTool)),
                };
            }
            set
            {
                var editedModuleListIndex = this.EditedPageDepositVM.EditedModuleListIndex;
                switch (this.EditedPageDepositVM.CurrentManagementTool)
                {
                    case PageManagementTool.PageLayoutComposer:
                    case PageManagementTool.TextWallLeftAsideEditor:
                    case PageManagementTool.TextWallRightAsideEditor:
                        break;
                    case PageManagementTool.TextWallSectionEditor:
                        this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.SectionWidth = value.HasValue ?
                            value.Value : Constants.DEFAULT_TEXT_WALL_SECTION_WIDTH;
                        break;
                    default:
                        throw ExceptionMaker.Member.Invalid(this.EditedPageDepositVM.CurrentManagementTool, nameof(this.EditedPageDepositVM.CurrentManagementTool));
                }
            }
        }
        #endregion


        #region Ctor & init
        public PageEditorViewModel(IDataDepository depository, IAuthService authService, IManagePageService managePageService, IManageFileService manageFileService)
        {
            this._depository        = depository;
            this._authService       = authService;
            this._managePageService = managePageService;
            this._manageFileService = manageFileService;

            this._currentlyEditedHeadingBannerVM = null;
            this._arePermissionsForSpecifiedManagementProvided = true;

            this._delayedDepositoryUpdateInvoker = new DelayedInvoker(
                Constants.MARKDOWN_EDITOR_DELAY_FOR_UPDATING_DEPOSITORY_MILLIS,
                this.UpdatePageDeposit
            );
        }
        public async Task LoadDataFromDeposit()
        {
            var gotDeposit = await this._depository.GetAsync<EditedPageDeposit>();

            bool managingPossible = gotDeposit != null;
            if (managingPossible)
            {
                EditedPageDepositVM = new EditedPageDepositViewModel(gotDeposit, this._managePageService);
                EditedPageDepositVM.PropertyChanged += (_, e) => this.NotifyPropertyChanged(e.PropertyName);
                EditedPageDepositVM.OnDataSet += (_, _e) => this._delayedDepositoryUpdateInvoker.DelayedInvoke();

                await this.ResolveManagementClearancesAsync(gotDeposit.PageDetails.PageType, gotDeposit.PageEditorMode);
            }
            IsManagementInfoProvided = managingPossible;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Move module content item up or down.
        /// </summary>
        /// <param name="menuItem">Moved item</param>
        /// <param name="moveUp"><c>true</c> - up, <c>false</c> - down</param>
        public void MoveModuleItem(GridItem<ModuleContent> menuItem, bool moveUp) =>
            this.EditedPageDepositVM.MoveModuleItem(menuItem, moveUp);
        
        /// <summary>
        /// Remove a module from collection in the deposited data and send changes into the depository.
        /// </summary>
        /// <param name="removedItem">The reference onto the removed module</param>
        public void RemoveModule(GridItem<ModuleContent> removedItem)
        {
            this.EditedPageDepositVM.ModuleContents.Remove(removedItem);
            this.UpdatePageDeposit();
        }
        /// <summary>
        /// Add a new module into the colledtion and edit it.
        /// </summary>
        /// <param name="moduleType">Type of the added module.</param>
        public void AddModule(PageModuleType moduleType)
        {
            this.ResetEditedModulesViewModels();
            PageManagementTool nextTool;
            var module = new ModuleContent();
            switch (moduleType)
            {
                case PageModuleType.HeadingBanner:
                    module.SetModule(new HeadingBannerModuleContent()
                    {
                        AuthorsInfo = null,
                        Description = null,
                        BackgroundPictureAccessGuid = null,
                        MenuItems           = null,
                        Title               = null,
                        PublishUtcTimestamp = DateTime.MinValue,
                        DarkDescription     = false,
                        ViewportHeight      = Constants.DEFAULT_HEADING_BANNER_HEIGHT,
                        DisplayAuthorsInfo  = true,
                        DisplayDescription  = true,
                        AttachLinkMenu      = true
                    });
                    nextTool = PageManagementTool.HeadingBannerEditor;
                    break;

                case PageModuleType.TextWall:
                    module.SetModule(new TextWallModuleContent()
                    {
                        LeftAsideMarkDownContent = string.Empty,
                        RightAsideMarkDownContent = string.Empty,
                        SectionMarkDownContent = string.Empty,
                        SectionWidth = Constants.DEFAULT_TEXT_WALL_SECTION_WIDTH
                    });
                    nextTool = PageManagementTool.TextWallSectionEditor;
                    break;

                case PageModuleType.PostListing:
                    module.SetModule(new PostListingModuleContent()
                    {
                        Width = PostListingWidth._800
                    });
                    nextTool = PageManagementTool.PostListingEditor;
                    break;

                default:
                    throw new ArgumentException($"The value {moduleType} of enum {nameof(PageModuleType)} is handled.", nameof(moduleType));
            }
            this.EditedPageDepositVM.ModuleContents.Add(new GridItem<ModuleContent>(module));
            this.EditedPageDepositVM.EditedModuleListIndex = this.EditedPageDepositVM.ModuleContents.Count - 1;
            this.EditedPageDepositVM.CurrentManagementTool = nextTool;
            this.UpdatePageDeposit();
        }
        /// <summary>
        /// Edit the selected module of a text wall
        /// </summary>
        /// <param name="item">Reference on the grid item</param>
        /// <param name="textWallEditedPart">Part of the text wall selected to be edited</param>
        public void EditModule(GridItem<ModuleContent> item, TextWallEditedPart textWallEditedPart)
        {
            var moduleType = item.Data.GetModuleType();
            if (moduleType != PageModuleType.TextWall)
            {
                throw ExceptionMaker.Member.Invalid(item, nameof(moduleType));
            }

            this.EditedPageDepositVM.EditedModuleListIndex = this.EditedPageDepositVM.ModuleContents.IndexOf(item);
            this.EditedPageDepositVM.CurrentManagementTool = textWallEditedPart.GetPageManagementTool();
            this.UpdatePageDeposit();
        }
        /// <summary>
        /// Edit the selected module.
        /// </summary>
        /// <param name="item">Reference on the grid item</param>
        public void EditModule(GridItem<ModuleContent> item)
        {
            this.ResetEditedModulesViewModels();
            var moduleType = item.Data.GetModuleType();
            if (moduleType == PageModuleType.TextWall)
            {
                throw ExceptionMaker.Member.Invalid(item, nameof(moduleType));
            }

            this.EditedPageDepositVM.EditedModuleListIndex = this.EditedPageDepositVM.ModuleContents.IndexOf(item);
            this.EditedPageDepositVM.CurrentManagementTool = item.Data.GetModuleType().GetPageManagementTool();
            this.UpdatePageDeposit();
        }
        /// <summary>
        /// Fired when the editor component fires exiting action.
        /// </summary>
        public void OnEditorExiting(object _, EventArgs e) =>
            BackToLayoutComposer();

        /// <summary>
        /// Change the currently displayed view to the page composer.
        /// </summary>
        public void BackToLayoutComposer()
        {
            this.EditedPageDepositVM.CurrentManagementTool = PageManagementTool.PageLayoutComposer;
            this.UpdatePageDeposit();
        }
        /// <summary>
        /// Fired when MarkDown code was changed.
        /// </summary>
        /// <param name="sender">Object invoking the event</param>
        /// <param name="code">Changed MarkDown code</param>
        public void OnMarkDownCodeChanged(object _, string code)
        {
            this.CurrentlyEditedMarkDownCode = code;
            this._delayedDepositoryUpdateInvoker.DelayedInvoke();
        }
        /// <summary>
        /// Fired when display width is changed.
        /// </summary>
        /// <param name="_">Object invoking the event</param>
        /// <param name="width">New chosen width of display</param>
        public void OnMaxPreviewedPageWidthChanged(object _, TextWallSectionWidth? width)
        {
            this.CurrentlyEditedSectionWidth = width;
            this.UpdatePageDeposit();
        }
        /// <summary>
        /// Apply all changes of the page.
        /// </summary>
        /// <returns>Task executing the apply</returns>
        public async Task ApplyChanges()
        {
            if (await this.ValidateAndInform())
            {
                switch (this.EditedPageDepositVM.PageEditorMode)
                {
                    case DataEditorMode.Create:
                        await this._managePageService.AddPage(this.EditedPageDepositVM.GetDetailedPageInfo());
                        break;

                    case DataEditorMode.Edit:
                        await this._managePageService.UpdatePage(this.EditedPageDepositVM.GetDetailedPageInfo());
                        break;

                    default:
                        throw new InvalidOperationException($"{this.EditedPageDepositVM.PageEditorMode} is not allowed.");
                }
                await this._depository.RemoveAsync<EditedPageDeposit>();
                this.OnExitingView?.Invoke();
            }
        }

        /// <summary>
        /// Make a URI part from entered title.
        /// </summary>
        public void CreateUriNameFromTitle() =>
            this.EditedPageDepositVM.UriName = SanitiseHelper.GetStringReadyForUri(this.EditedPageDepositVM.Title.Truncate(150));

        public void ResetEditedModulesViewModels() =>
            this._currentlyEditedHeadingBannerVM = null;
        #endregion

        #region Helper methods
        /// <summary>
        /// Check validity of the entered data and inform a user
        /// by setting proper messages.
        /// </summary>
        /// <returns>Task returning if data is valid</returns>
        private async Task<bool> ValidateAndInform() =>
            await this.EditedPageDepositVM.ValidateAndInformAsync();

        /// <summary>
        /// Update data in the edited page's data deposit.
        /// </summary>
        private void UpdatePageDeposit() =>
            this._depository.AddOrUpdateAsync(this.EditedPageDepositVM.ToDeposit());

        /// <summary>
        /// Check if there are any policies that block creating or editing the page data
        /// depending on the provided work settings.
        /// </summary>
        /// <param name="editorMode">Data editor work mode</param>
        /// <returns>The executing task</returns>
        private async Task ResolveManagementClearancesAsync(PageType pageType, DataEditorMode editorMode)
        {
            var expectedClearance = EnumExtensions.GetClearanceOfPageEditorUsage(pageType, editorMode);
            this.ArePermissionsForSpecifiedManagementProvided = await this._authService.HasClearanceAsync(expectedClearance);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this._delayedDepositoryUpdateInvoker != null)
            {
                this._delayedDepositoryUpdateInvoker.Dispose();
                this._delayedDepositoryUpdateInvoker = null;
            }
        }
        ~PageEditorViewModel() => this.Dispose();
        #endregion
    }
}
