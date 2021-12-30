using Core.Application.Helpers;
using Core.Application.Helpers.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers.Extensions;
using Core.Shared.Models;
using Core.Shared.Models.ManageFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Adapters;
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
        private int? _currentParentNodeId;
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

        #region Properties :: Handled from code behind
        /// <summary>
        /// The list holding info about files for upload.
        /// Data set from the view's code behind.
        /// </summary>
        public IReadOnlyList<IBrowserFile> FilesForUpload { get; set; }
        /// <summary>
        /// Indicates if files ready for upload are not set.
        /// </summary>
        public bool FilesForUploadUnset =>
            this.FilesForUpload == null || this.FilesForUpload.Count == 0;
        #endregion

        #region Properties :: Event handlers
        /// <summary>
        /// Events raised when the upload is begun.
        /// </summary>
        public EventHandler OnFilesUploadBegin { get; set; }
        /// <summary>
        /// Events raised when a quantity of successfully uploaded files is changed.
        /// </summary>
        public EventHandler<int> OnFilesUploadSingleFileSuccess { get; set; }
        /// <summary>
        /// Events raised whenever files upload finishes with fail.
        /// Quantity of upload failures is passed.
        /// </summary>
        public EventHandler<int> OnFilesUploadFailure { get; set; }
        /// <summary>
        /// Events raised after successful sending of all files.
        /// </summary>
        public EventHandler OnFilesUploadSuccess { get; set; }
        #endregion

        #region Ctor
        public ManageFilesViewModel(IManageFileService manageFileService)
        {
            this._currentParentNodeId = null;
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
            this.PrepareFilesFetcher(this._currentParentNodeId);
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

        public async Task<bool> CreateDirectoryForCurrentNodeAsync(string name)
        {
            var response = await this._manageFileService.CreateLogicalDirectory(name, this._currentParentNodeId);
            if (response.GetValue(out SimpleFileInfo file, out _))
            {
                this.Files.AddAndPack(file);
                this.NotifyPropertyChanged(nameof(Files));
                return true;
            }
            return false;
        }

        public async Task EnterDirectoryAsync(SimpleFileInfo fileInfo)
        {
            this._currentParentNodeId = fileInfo.Name == ".." ?
                fileInfo.ParentFileId :
                fileInfo.Id;

            this._filesFetcher = this._manageFileService.GetSimpleFileInfos(this._currentParentNodeId, this._fileSearchParams);
            await this.LoadFilesAsync();
        }



        public async Task UploadFilesAsync()
        {
            if (this.FilesForUploadUnset)
            {
                return;
            }
            this.OnFilesUploadBegin.Invoke(this, EventArgs.Empty);

            int countOfUploadFailures = 0;
            List<SimpleFileInfo> addedFiles = new();
            var adapteredFilesInfos = this.FilesForUpload.Select(i => new ClientUploadedFileAdapter(i));

            await this._manageFileService.UploadFiles(adapteredFilesInfos, this._currentParentNodeId,
                new Action<Result<SimpleFileInfo, object>>(partialResult =>
                {
                    if (partialResult.GetValue(out SimpleFileInfo successfulFile, out _))
                    {
                        addedFiles.Add(successfulFile);
                        this.OnFilesUploadSingleFileSuccess.Invoke(this, addedFiles.Count);
                    }
                    else
                    {
                        ++countOfUploadFailures;
                    }
                }));

            this.Files.AddRangeAndPack(addedFiles);
            this.NotifyPropertyChanged(nameof(Files));
            
            if (countOfUploadFailures == 0)
            {
                this.OnFilesUploadSuccess.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.OnFilesUploadFailure.Invoke(this, countOfUploadFailures);
            }
            this.FilesForUpload = null;
        }
        #endregion
    }
}
