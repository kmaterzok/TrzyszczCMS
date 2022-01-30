using Core.Application.Helpers.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Models.LoadPage;
using Core.Shared.Models.PageContent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.PageContent.Modules
{
    /// <summary>
    /// The viewmodel for a heading banner page module.
    /// </summary>
    public class PostListingModuleViewModel : ViewModelBase
    {
        #region Fields
        private readonly ILoadPageService _loadPageService;
        private IPageFetcher<SimplePublicPostInfo> _postsFetcher;
        #endregion

        #region Properties
        private PostListingModuleContent _ModuleContent;
        /// <summary>
        /// Content of the module which the viewmodel gets data from.
        /// </summary>
        public PostListingModuleContent ModuleContent
        {
            get => _ModuleContent;
            set => Set(ref _ModuleContent, value, nameof(ModuleContent));
        }

        private List<SimplePublicPostInfo> _posts;
        /// <summary>
        /// All loaded and displayed posts.
        /// </summary>
        public List<SimplePublicPostInfo> Posts
        {
            get => _posts;
            set => Set(ref _posts, value, nameof(Posts));
        }
        /// <summary>
        /// Indicates whether there are more articles to be fetched.
        /// </summary>
        public string CssClassForLoadMorePosts =>
            CssClassesHelper.ClassCollapsingElement(this._postsFetcher.HasNext);
        #endregion

        #region Ctor
        public PostListingModuleViewModel(PostListingModuleContent moduleContent, ILoadPageService loadPageService)
        {
            this._loadPageService = loadPageService;
            this.ResetFetcher();
            this.Posts = new List<SimplePublicPostInfo>();
            this.ModuleContent = moduleContent;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load the first page of infos about posts to <see cref="Posts"/>.
        /// </summary>
        /// <returns>Task executing the operation</returns>
        public async Task FetchFirstPageOfPostInfosAsync()
        {
            this.ResetFetcher();
            var datapage = await this._postsFetcher.GetCurrent();
            this.Posts = datapage.Entries;
            this.NotifyPropertyChanged(nameof(CssClassForLoadMorePosts));
        }
        /// <summary>
        /// Load the very next page of infos about posts and
        /// append the fetched data into <see cref="Posts"/>.
        /// </summary>
        /// <returns>Task executing the operation</returns>
        public async Task FetchNextPageAsync()
        {
            var datapage = await this._postsFetcher.GetNext();
            this.Posts.AddRange(datapage.Entries);
            this.NotifyPropertyChanged(nameof(this.Posts));
            this.NotifyPropertyChanged(nameof(CssClassForLoadMorePosts));
        }
        #endregion

        #region Helper methods
        private void ResetFetcher() =>
            this._postsFetcher = this._loadPageService.GetSimplePublicPostInfos();
        #endregion
    }
}
