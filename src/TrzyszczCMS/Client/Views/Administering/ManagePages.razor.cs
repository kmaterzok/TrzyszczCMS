using System;
using System.Collections.Generic;
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
            this.DisplayPosts();
            base.OnInitialized();
        }
        #endregion

        #region Link methods
        private void EnableButtons()
        {
            postsButtonEnabled = true;
            articlesButtonEnabled = true;
        }

        private void DisplayPosts()
        {
            this.EnableButtons();
            postsButtonEnabled = false;
            this.ViewModel.LoadPosts();
        }
        private void DisplayArticles()
        {
            this.EnableButtons();
            articlesButtonEnabled = false;
            this.ViewModel.LoadArticles();
        }
        #endregion

        #region Other methods
        #endregion
    }
}
