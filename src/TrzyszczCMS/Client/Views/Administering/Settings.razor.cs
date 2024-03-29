﻿using TrzyszczCMS.Core.Shared.Models.ManageSettings;
using TrzyszczCMS.Core.Infrastructure.Shared.Data;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Administering
{
    public partial class Settings
    {
        #region Fields
        private bool? manageNavBarAllowed = null;
        #endregion

        #region Properties
        [CascadingParameter]
        private Popupper Popupper { get; set; }
        #endregion

        #region Init
        protected override async Task OnInitializedAsync()
        {
            this.ViewModel.PropertyChanged += new PropertyChangedEventHandler(
                async (s, e) => await this.InvokeAsync(() => this.StateHasChanged())
            );
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                this.manageNavBarAllowed = await AuthService.HasClearanceAsync(PolicyClearance.AccessNavBarSettings);
                if (true == this.manageNavBarAllowed)
                {
                    await this.ViewModel.LoadMenuItemsAsync();
                }
            }
        }
        #endregion

        #region Methods
        private void AddMenuItem()
        {
            this.Popupper.ShowPrompt("Enter a name of the new item:", nameResult =>
            {
                if (nameResult.GetValue(out string name, out _))
                {
                    if (name == "..")
                    {
                        this.Popupper.ShowAlert("This name is forbidden.");
                        return;
                    }
                    this.Popupper.ShowPrompt("Enter a URI for the item:", async uriResult =>
                    {
                        if (uriResult.GetValue(out string uri, out _))
                        {
                            await this.ViewModel.AddMenuItemAsync(new SimpleMenuItemInfo()
                            {
                                Name = name,
                                Uri  = uri,
                                ParentItemId = this.ViewModel.CurrentMenuItemsParentNodeId
                            });
                        }
                    }, true, maxLength: Constraints.ContMenuItem.URI);
                }
            }, true, maxLength: Constraints.ContMenuItem.NAME);
        }

        private void DeleteMenuItem(SimpleMenuItemInfo menuItem)
        {
            this.Popupper.ShowYesNoPrompt($"Delete <em>{menuItem.Name}</em>?", async result =>
            {
                if (result == PopupExitResult.Yes)
                {
                    await this.ViewModel.DeleteMenuItemAsync(menuItem.Id);
                }
            });
        }
        #endregion
    }
}
