using TrzyszczCMS.Core.Application.Helpers;
using TrzyszczCMS.Core.Application.Helpers.Interfaces;
using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManagePage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.ViewModels.Shared;
using TrzyszczCMS.Client.Views.Administering;
using TrzyszczCMS.Core.Shared.Helpers.Extensions;
using TrzyszczCMS.Core.Application.Models.Deposits;
using TrzyszczCMS.Core.Application.Enums;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Core.Shared.Helpers;

namespace TrzyszczCMS.Client.ViewModels.Administering
{
    /// <summary>
    /// Viewmodel containing data for displaying in <see cref="ManagePages"/> view.
    /// </summary>
    public class ManagePagesViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDataDepository _depository;
        private readonly IManagePageService _managePageService;
        private readonly Dictionary<FilteredGridField, string> _postSearchParams;
        private readonly Dictionary<FilteredGridField, string> _articleSearchParams;

        private IPageFetcher<SimplePageInfo> _postsFetcher;
        private IPageFetcher<SimplePageInfo> _articlesFetcher;
        private bool _postsLoaded;
        private bool _articlesLoaded;
        #endregion

        #region Properties
        private List<GridItem<SimplePageInfo>> _posts;
        /// <summary>
        /// All posts currently displayed in the posts grid.
        /// </summary>
        public List<GridItem<SimplePageInfo>> Posts
        {
            get => _posts;
            set => Set(ref _posts, value, nameof(Posts));
        }

        private List<GridItem<SimplePageInfo>> _articles;
        /// <summary>
        /// All articles currently displayed in the articles grid.
        /// </summary>
        public List<GridItem<SimplePageInfo>> Articles
        {
            get => _articles;
            set => Set(ref _articles, value, nameof(Articles));
        }

        private bool _canFetchPosts;
        /// <summary>
        /// Possibility of fetching more information about posts.
        /// </summary>
        public bool CanFetchPosts
        {
            get => _canFetchPosts;
            set => Set(ref _canFetchPosts, value, nameof(CanFetchPosts));
        }
        
        private bool _canFetchArticles;
        /// <summary>
        /// /// Possibility of fetching more information about articles.
        /// </summary>
        public bool CanFetchArticles
        {
            get => _canFetchArticles;
            set => Set(ref _canFetchArticles, value, nameof(CanFetchArticles));
        }

        /// <summary>
        /// Indicates whether there are more articles to be fetched.
        /// </summary>
        public string CssClassForLoadMoreArticles =>
            CssClassesHelper.ClassCollapsingElement(this._articlesFetcher.HasNext);

        /// <summary>
        /// Indicates whether there are more articles to be fetched.
        /// </summary>
        public string CssClassForLoadMorePosts =>
            CssClassesHelper.ClassCollapsingElement(this._postsFetcher.HasNext);
        #endregion

        #region Ctor
        public ManagePagesViewModel(IDataDepository depository, IManagePageService managePageService)
        {
            this.Posts = new List<GridItem<SimplePageInfo>>();
            this.Articles = new List<GridItem<SimplePageInfo>>();
            this._postSearchParams    = new Dictionary<FilteredGridField, string>();
            this._articleSearchParams = new Dictionary<FilteredGridField, string>();
            this._postsLoaded = this._articlesLoaded = this.CanFetchArticles = this.CanFetchPosts = false;

            this._depository = depository;
            this._managePageService = managePageService;

            this.PreparePostsFetcher();
            this.PrepareArticlesFetcher();
        }
        #endregion

        #region Methods
        public async Task LoadFirstPageOfPosts(bool force = false)
        {
            if (this._postsLoaded && !force)
            {
                return;
            }
            this.Posts = (await this._postsFetcher.GetCurrent()).Entries.ToGridItemList();
            this.CanFetchPosts = this._postsFetcher.HasNext;
            this._postsLoaded = true;
        }

        public async Task LoadFirstPageOfArticles(bool force = false)
        {
            if (this._articlesLoaded && !force)
            {
                return;
            }
            this.Articles = (await this._articlesFetcher.GetCurrent()).Entries.ToGridItemList();
            this.CanFetchArticles = this._articlesFetcher.HasNext;
            this._articlesLoaded = true;
        }

        #region Searching pages
        public void OnPostsSearch(ValueRange<DateTime?> range, FilteredGridField columnTitle) =>
            this._postSearchParams.AddOrUpdate(columnTitle, range.MakeDateRangeFilterString());

        public void OnPostsSearch(ChangeEventArgs changeEventArgs, FilteredGridField columnTitle) =>
            this._postSearchParams.AddOrUpdate(columnTitle, changeEventArgs.Value.ToString());
        
        public void OnArticlesSearch(ChangeEventArgs changeEventArgs, FilteredGridField columnTitle) =>
            this._articleSearchParams.AddOrUpdate(columnTitle, changeEventArgs.Value.ToString());

        public void OnArticlesSearch(ValueRange<DateTime?> range, FilteredGridField columnTitle) =>
            this._articleSearchParams.AddOrUpdate(columnTitle, range.MakeDateRangeFilterString());
        #endregion

        public async Task ApplySearchAsync(PageType type)
        {
            switch (type)
            {
                case PageType.Post:
                    this.PreparePostsFetcher();
                    await LoadFirstPageOfPosts(true);
                    break;

                case PageType.Article:
                    this.PrepareArticlesFetcher();
                    await LoadFirstPageOfArticles(true);
                    break;
            }
        }

        public async Task SendDataToDepositoryForEditingAsync(PageType pagetype, int? editedPageId = null)
        {
            DetailedPageInfo pageInfo;
            switch (pagetype)
            {
                case PageType.HomePage:
                    pageInfo = await this._managePageService.GetDetailedPageInfoOfHomepage();
                    break;

                case PageType.Post:
                case PageType.Article:
                    pageInfo = await this._managePageService.GetDetailedPageInfo(editedPageId.Value);
                    break;

                default:
                    throw ExceptionMaker.NotImplemented.ForHandling(pagetype, nameof(pagetype));
            }
            await this._depository.AddOrUpdateAsync(new EditedPageDeposit()
            {
                PageEditorMode        = DataEditorMode.Edit,
                PageDetails           = pageInfo,
                EditedModuleListIndex = 0,
                CurrentManagementTool = PageManagementTool.PageLayoutComposer,
                OldUriName            = pageInfo.UriName
            });
        }

        public async Task SendDataToDepositoryForCreatingAsync(PageType pagetype)
        {
            switch (pagetype)
            {
                case PageType.Post:
                case PageType.Article:
                    await this._depository.AddOrUpdateAsync(new EditedPageDeposit()
                    {
                        PageEditorMode        = DataEditorMode.Create,
                        PageDetails           = DetailedPageInfo.MakeEmpty(pagetype),
                        EditedModuleListIndex = 0,
                        CurrentManagementTool = PageManagementTool.PageLayoutComposer,
                        OldUriName            = null
                    });
                    break;

                case PageType.HomePage:
                    throw ExceptionMaker.Argument.Invalid(pagetype, nameof(pagetype));
                default:
                    throw ExceptionMaker.Argument.Unsupported(pagetype, nameof(pagetype));
            }
        }

        public async Task FetchPageDataNextPage(PageType type)
        {
            switch (type)
            {
                case PageType.Article:
                    var nextArticles = await this._articlesFetcher.GetNext();
                    this.Articles.AddRangeAndPack(nextArticles.Entries);
                    this.NotifyPropertyChanged(nameof(this.Articles));
                    break;

                case PageType.Post:
                    var nextPosts = await this._postsFetcher.GetNext();
                    this.Posts.AddRangeAndPack(nextPosts.Entries);
                    this.NotifyPropertyChanged(nameof(this.Posts));
                    break;

                default:
                    throw ExceptionMaker.Argument.Unsupported(type, nameof(type));
            }
        }

        public async Task<bool> DeleteSelectedPagesAsync(PageType type)
        {
            var deletedPagesIds = this.GetPagesByType(type).Where(i => i.Checked)
                                                           .Select(i => i.Data.Id)
                                                           .ToArray();
            if (deletedPagesIds.Length > 0)
            {
                await this._managePageService.DeletePages(deletedPagesIds);
                return true;
            }
            return false;
        }

        public async Task DeletePageAsync(int pageId) =>
            await this._managePageService.DeletePages(pageId);

        /// <summary>
        /// Retrieve pages by type
        /// </summary>
        /// <param name="type">Type of retrieved pages</param>
        /// <returns>List of pages of certain type</returns>
        public List<GridItem<SimplePageInfo>> GetPagesByType(PageType type) => type switch
        {
            PageType.Article => this.Articles,
            PageType.Post    => this.Posts,
            _ => throw ExceptionMaker.Argument.Unsupported(type, nameof(type)),
        };
        #endregion

        #region Helpers
        private void PreparePostsFetcher() =>
            this._postsFetcher = this._managePageService.GetSimplePageInfos(PageType.Post, this._postSearchParams);
        
        private void PrepareArticlesFetcher() =>
            this._articlesFetcher = this._managePageService.GetSimplePageInfos(PageType.Article, this._articleSearchParams);
        #endregion
    }
}
