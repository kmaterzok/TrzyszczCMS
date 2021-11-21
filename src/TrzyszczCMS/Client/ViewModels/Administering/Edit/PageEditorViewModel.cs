using Core.Application.Enums;
using Core.Application.Helpers;
using Core.Application.Models.Deposits;
using Core.Shared.Enums;
using Core.Shared.Exceptions;
using Core.Shared.Models.PageContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private DelayedInvoker _delayedDepositoryUpdateInvoker;
        #endregion


        #region Properties
        private bool _isManagingPossible;
        /// <summary>
        /// It says whether there is data allowing to manage a page.
        /// </summary>
        public bool IsManagingPossible
        {
            get => _isManagingPossible;
            set => Set(ref _isManagingPossible, value, nameof(IsManagingPossible));
        }

        private EditedPageDepositViewModel _editedPageDepositVM;
        /// <summary>
        /// Remapped instance of <see cref="EditedPageDepositVM"/> for editing purposes.
        /// </summary>
        public EditedPageDepositViewModel EditedPageDepositVM
        {
            get => _editedPageDepositVM;
            set => Set(ref _editedPageDepositVM, value, nameof(EditedPageDepositViewModel));
        }

        /// <summary>
        /// The MarkDown editor's edited code.
        /// </summary>
        public string CurrentlyEditedMarkDownCode
        {
            get
            {
                var editedModuleListIndex = this.EditedPageDepositVM.EditedModuleListIndex;
                switch (this.EditedPageDepositVM.CurrentManagementTool)
                {
                    case PageManagementTool.PageLayoutComposer:
                        return string.Empty;
                    case PageManagementTool.TextWallSectionEditor:
                        return this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.SectionMarkDownContent;
                    case PageManagementTool.TextWallLeftAsideEditor:
                        return this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.LeftAsideMarkDownContent;
                    case PageManagementTool.TextWallRightAsideEditor:
                        return this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.RightAsideMarkDownContent;
                    default:
                        throw new InvalidMemberException($"The value {this.EditedPageDepositVM.CurrentManagementTool} of enum {nameof(PageManagementTool)} is not handled.");
                }
            }
            set
            {
                Console.WriteLine($"Writing start. Len: {value.Length}");
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
                        throw new InvalidMemberException($"The value {this.EditedPageDepositVM.CurrentManagementTool} of enum {nameof(PageManagementTool)} is not handled.");
                }
                Console.WriteLine($"Writing end. Len: {value.Length}");
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
                switch (this.EditedPageDepositVM.CurrentManagementTool)
                {
                    case PageManagementTool.PageLayoutComposer:
                    case PageManagementTool.TextWallLeftAsideEditor:
                    case PageManagementTool.TextWallRightAsideEditor:
                        return null;
                    case PageManagementTool.TextWallSectionEditor:
                        return this.EditedPageDepositVM.ModuleContents[editedModuleListIndex].Data.TextWallModuleContent.SectionWidth;
                    default:
                        throw new InvalidMemberException($"The value {this.EditedPageDepositVM.CurrentManagementTool} of enum {nameof(PageManagementTool)} is not handled.");
                }
            }
            set
            {
                Set(() =>
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
                            throw new InvalidMemberException($"The value {this.EditedPageDepositVM.CurrentManagementTool} of enum {nameof(PageManagementTool)} is not handled.");
                    }
                }, nameof(CurrentlyEditedSectionWidth));
            }
        }
        #endregion


        #region Ctor & init
        public PageEditorViewModel(IDataDepository depository)
        {
            this._depository = depository;
            this._delayedDepositoryUpdateInvoker = new DelayedInvoker(
                Constants.MARKDOWN_EDITOR_DELAY_FOR_UPDATING_DEPOSITORY_MILLIS,
                this.UpdatePageDeposit
            );
        }
        public async Task LoadDataFromDeposit()
        {
            EditedPageDepositVM = new EditedPageDepositViewModel(await this._depository.GetAsync<EditedPageDeposit>());
            EditedPageDepositVM.PropertyChanged += (_, e) => this.NotifyPropertyChanged(e.PropertyName);

            IsManagingPossible = _editedPageDepositVM != null;
        }
        #endregion

        #region Methods
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
            PageManagementTool nextTool;
            var module = new ModuleContent();
            switch (moduleType)
            {
                case PageModuleType.Gallery:
                    //nextTool = PageManagementTool.PageLayoutComposer;
                    // TODO: Implement
                    return;
                    //break;

                case PageModuleType.HeadingBanner:
                    //nextTool = PageManagementTool.PageLayoutComposer;
                    // TODO: Implement
                    return;
                    //break;

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
                throw new ArgumentException($"The moudle type other than {nameof(PageModuleType.TextWall)} is not supported.", nameof(item));
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
            throw new NotImplementedException();
        }
        /// <summary>
        /// Fired when the editor component fires exiting action.
        /// </summary>
        public void OnEditorExiting(object _, EventArgs e)
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
            this._delayedDepositoryUpdateInvoker.Invoke();
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Update data in the edited page's data deposit.
        /// </summary>
        private void UpdatePageDeposit() =>
            this._depository.AddOrUpdateAsync(this.EditedPageDepositVM.ToDeposit());
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
