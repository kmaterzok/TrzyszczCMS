﻿using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Models.ManagePage;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class ManagePages
    {
        #region Fields
        private bool postsButtonEnabled;
        private bool articlesButtonEnabled;

        private bool  displayingHomepageAllowed = false;
        private bool? displayingArticlesAllowed = null;
        private bool? displayingPostsAllowed    = null;

        private bool bulkDeletePageButtonDisabled = false;
        private bool addPageButtonDisabled        = false;

        private bool editingPostsDisallowed     = false;
        private bool editingArticlesDisallowed  = false;
        private bool deletingPostsDisallowed    = false;
        private bool deletingArticlesDisallowed = false;
        #endregion

        #region Properties
        [CascadingParameter]
        private Popupper Popupper { get; set; }

        private string PostsButtonLinkEnableClasses =>
            CssClassesHelper.ClassesForLink(this.postsButtonEnabled && true == this.displayingPostsAllowed);

        private string ArticlesButtonLinkEnableClasses =>
            CssClassesHelper.ClassesForLink(this.articlesButtonEnabled && true == this.displayingArticlesAllowed);

        private string HomepageButtonLinkPermitClasses =>
            CssClassesHelper.ClassesForLink(this.displayingHomepageAllowed);

        private PageType CurrentlyManagedPageType
        {
            get
            {
                if      (!this.postsButtonEnabled)    { return PageType.Post; }
                else if (!this.articlesButtonEnabled) { return PageType.Article; }                
                throw new InvalidOperationException("Current state of page does not let to tell the current type of managed pages.");
            }
        }

        private bool DisplayArticlesGrid =>
            !this.postsButtonEnabled && this.articlesButtonEnabled;

        private bool DisplayPostsGrid =>
            this.postsButtonEnabled && !this.articlesButtonEnabled;
        #endregion

        #region Init
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );

            this.displayingHomepageAllowed  = await AuthService.HasClearanceAsync(PolicyClearance.DisplayHomepageForManaging);
            this.displayingArticlesAllowed  = await AuthService.HasClearanceAsync(PolicyClearance.DisplayArticlesForManaging);
            this.displayingPostsAllowed     = await AuthService.HasClearanceAsync(PolicyClearance.DisplayPostsForManaging);
            this.editingArticlesDisallowed  = !await AuthService.HasClearanceAsync(PolicyClearance.AllowArticlesEditing);
            this.editingPostsDisallowed     = !await AuthService.HasClearanceAsync(PolicyClearance.AllowPostsEditing);
            this.deletingArticlesDisallowed = !await AuthService.HasClearanceAsync(PolicyClearance.AllowArticlesDeleting);
            this.deletingPostsDisallowed    = !await AuthService.HasClearanceAsync(PolicyClearance.AllowPostsDeleting);
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
            await this.RefreshButtonsDisableStatus();
            await this.ViewModel.LoadFirstPageOfPosts();
        }
        private async Task DisplayArticles()
        {
            this.EnableButtons();
            articlesButtonEnabled = false;
            await this.RefreshButtonsDisableStatus();
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
        private async Task RefreshButtonsDisableStatus()
        {
            this.bulkDeletePageButtonDisabled = !await AuthService.HasClearanceAsync(this.CurrentlyManagedPageType.GetClearanceOfPageDeleting());
            this.addPageButtonDisabled        = !await AuthService.HasClearanceAsync(this.CurrentlyManagedPageType.GetClearanceOfPageAdding());
        }

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
