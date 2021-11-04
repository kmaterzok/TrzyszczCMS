using Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManagePages
    {
        #region Fields & properties
        private bool postsButtonEnabled;
        private bool articlesButtonEnabled;

        private string PostsButtonLinkEnableClasses
        {
            get => CssClassesHelper.ClassesForLink(this.postsButtonEnabled);
        }
        private string ArticlesButtonLinkEnableClasses
        {
            get => CssClassesHelper.ClassesForLink(this.articlesButtonEnabled);
        }
        #endregion

        #region Init
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await this.DisplayPosts();
            }
        }
        #endregion

        #region Link methods
        private void EnableButtons()
        {
            postsButtonEnabled = true;
            articlesButtonEnabled = true;
        }

        private async Task DisplayPosts()
        {
            this.EnableButtons();
            postsButtonEnabled = false;
            await this.ViewModel.LoadFirstPageOfPosts();
        }
        private async Task DisplayArticles()
        {
            this.EnableButtons();
            articlesButtonEnabled = false;
            await this.ViewModel.LoadFirstPageOfArticles();
        }
        private void GoToManagingHomepage()
        {
            // TODO: To implementation.
        }
        #endregion

        #region Other methods
        private async Task ApplySearchAsync()
        {
            if (!this.postsButtonEnabled)
            {
                await this.ViewModel.ApplySearchAsync(PageType.Post);
            }
            else if (!this.articlesButtonEnabled)
            {
                await this.ViewModel.ApplySearchAsync(PageType.Article);
            }
        }
        #endregion
    }
}
