using Core.Shared.Enums;
using Core.Shared.Models.ManagePage;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManagePages
    {
        #region Fields & properties
        private bool postsButtonEnabled;
        private bool articlesButtonEnabled;

        [CascadingParameter]
        private Popupper Popupper { get; set; }

        private string PostsButtonLinkEnableClasses =>
            CssClassesHelper.ClassesForLink(this.postsButtonEnabled);
        private string ArticlesButtonLinkEnableClasses =>
            CssClassesHelper.ClassesForLink(this.articlesButtonEnabled);
        private PageType CurrentlyManagedPageType
        {
            get
            {
                if      (!this.postsButtonEnabled)    { return PageType.Post; }
                else if (!this.articlesButtonEnabled) { return PageType.Article; }                
                throw new InvalidOperationException("Current state of page does not let to tell the current type of managed pages.");
            }
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
        private async Task GoToManagingHomepageAsync()
        {
            await this.ViewModel.SendDataToDepositoryForEditingAsync(PageType.HomePage);
            this.NavigationManager.NavigateTo("/manage/edit-page");
        }
        private async Task GoToCreatingPageAsync()
        {
            await this.ViewModel.SendDataToDepositoryForCreatingAsync(this.CurrentlyManagedPageType);
            this.NavigationManager.NavigateTo("/manage/edit-page");
        }
        private async Task GoToManagingPageAsync(PageType type, int pageId)
        {
            await this.ViewModel.SendDataToDepositoryForEditingAsync(type, pageId);
            this.NavigationManager.NavigateTo("/manage/edit-page");
        }
        #endregion

        #region Other methods
        private async Task ApplySearchAsync() =>
            await this.ViewModel.ApplySearchAsync(this.CurrentlyManagedPageType);

        private async Task LoadFirstPageOfPages()
        {
            switch (this.CurrentlyManagedPageType)
            {
                case PageType.Article:
                    await this.ViewModel.LoadFirstPageOfArticles(true);
                    break;

                case PageType.Post:
                    await this.ViewModel.LoadFirstPageOfPosts(true);
                    break;

                default:
                    break;
            }
        }

        private void DeleteSelectedPages()
        {
            var selectedPages = this.ViewModel.GetPagesByType(this.CurrentlyManagedPageType)
                                              .Where(i => i.Checked)
                                              .Select(i => $"<em>{i.Data.Title}</em>");
            if (!selectedPages.Any())
            {
                return;
            }
            var deletedPagesMessage = HtmlHelper.MakeUnorderedList(selectedPages);
            deletedPagesMessage.Insert(0, "Delete these pages?<br/>");

            this.Popupper.ShowYesNoPrompt(deletedPagesMessage.ToString(), async result =>
            {
                if (result == PopupExitResult.Yes)
                {
                    var anythingDeleted = await this.ViewModel.DeleteSelectedPagesAsync(this.CurrentlyManagedPageType);
                    if (anythingDeleted)
                    {
                        await this.LoadFirstPageOfPages();
                    }
                }
            });
        }

        private void DeletePage(SimplePageInfo page)
        {
            this.Popupper.ShowYesNoPrompt($"Delete <em>{page.Title}</em>?", async result =>
            {
                if (result == PopupExitResult.Yes)
                {
                    await this.ViewModel.DeletePageAsync(page.Id);
                    await this.LoadFirstPageOfPages();
                }
            });
        }
        #endregion
    }
}
