using Core.Application.Helpers;
using Core.Application.Helpers.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers.Extensions;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering
{
    public class ManageFilesViewModel : ViewModelBase
    {
        #region Fields
        private readonly IManageFileService _manageFileService;
        private readonly Dictionary<FilteredGridField, string> _fileSearchParams;

        private IPageFetcher<SimpleFileInfo> _filesFetcher;
        private int? _currentParentNode;
        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether there are more files to be fetched.
        /// </summary>
        public string CssClassForLoadMoreFiles =>
            CssClassesHelper.ClassCollapsingElement(this._filesFetcher.HasNext);

        private List<GridItem<SimpleFileInfo>> _files;
        /// <summary>
        /// All files currently displayed in the posts grid.
        /// These files come from a specified directory.
        /// </summary>
        public List<GridItem<SimpleFileInfo>> Files
        {
            get => _files;
            set => Set(ref _files, value, nameof(Files));
        }

        private bool _canFetchFiles;
        /// <summary>
        /// Possibility of fetching more information about files.
        /// </summary>
        public bool CanFetchFiles
        {
            get => _canFetchFiles;
            set => Set(ref _canFetchFiles, value, nameof(CanFetchFiles));
        }
        #endregion

        #region Ctor
        public ManageFilesViewModel(IManageFileService manageFileService)
        {
            this._currentParentNode = null;
            this._manageFileService = manageFileService;
            this._fileSearchParams = new Dictionary<FilteredGridField, string>();
            this.Files = new List<GridItem<SimpleFileInfo>>();
            this.PrepareFilesFetcher();
        }
        #endregion

        #region Methods :: Search
        private void PrepareFilesFetcher(int? parentFileNodeId = null) =>
            this._filesFetcher = this._manageFileService.GetSimpleFileInfos(parentFileNodeId, this._fileSearchParams);

        public void OnSearch(ValueRange<DateTime?> range, FilteredGridField columnTitle) =>
            this._fileSearchParams.AddOrUpdate(columnTitle, range.MakeDateRangeFilterString());

        public void OnSearch(ChangeEventArgs changeEventArgs, FilteredGridField columnTitle) =>
            this._fileSearchParams.AddOrUpdate(columnTitle, changeEventArgs.Value.ToString());
        #endregion

        #region Methods
        public async Task ApplySearchAsync()
        {
            this.PrepareFilesFetcher(this._currentParentNode);
            await LoadFilesAsync();
        }
        public async Task LoadFilesAsync()
        {
            this.Files = (await this._filesFetcher.GetCurrent()).Entries.ToGridItemList();
            this.CanFetchFiles = this._filesFetcher.HasNext;
        }

        public async Task FetchPageDataNextFiles()
        {
            var nextArticles = await this._filesFetcher.GetNext();
            this.Files.AddRangeAndPack(nextArticles.Entries);
            this.NotifyPropertyChanged(nameof(this.Files));
        }
        public async Task DeleteFileAsync(int fileId)
        {
            await this._manageFileService.DeleteFile(fileId);

            this.Files.Remove(this.Files.Single(i => i.Data.Id == fileId));
            this.NotifyPropertyChanged(nameof(this.Files));
        }
        #endregion
    }
}
