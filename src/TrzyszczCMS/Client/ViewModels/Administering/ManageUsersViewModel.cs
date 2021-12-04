using Core.Application.Enums;
using Core.Application.Models.Deposits;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Enums;
using Core.Shared.Helpers.Extensions;
using Core.Shared.Models.ManageUser;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering
{
    public class ManageUsersViewModel : ViewModelBase
    {
        #region Fields
        private readonly IManageUserService _manageUserService;
        private readonly IDataDepository _depository;
        private readonly Dictionary<FilteredGridField, string> _userSearchParams;
        #endregion

        #region Ctor
        public ManageUsersViewModel(IManageUserService manageUserService, IDataDepository depository)
        {
            this.Users = new List<GridItem<SimpleUserInfo>>();
            this._userSearchParams = new Dictionary<FilteredGridField, string>();
            this._manageUserService = manageUserService;
            this._depository = depository;
        }
        #endregion

        #region Properties
        private List<GridItem<SimpleUserInfo>> _users;
        /// <summary>
        /// All users currently displayed in the articles grid.
        /// </summary>
        public List<GridItem<SimpleUserInfo>> Users
        {
            get => _users;
            set => Set(ref _users, value, nameof(Users));
        }
        #endregion

        #region Methods
        public void OnUsersSearch(ChangeEventArgs changeEventArgs, FilteredGridField columnTitle) =>
            this._userSearchParams.AddOrUpdate(columnTitle, changeEventArgs.Value.ToString());

        public async Task DeleteUserAsync(int userId) =>
            await this._manageUserService.DeleteUser(userId);

        public async Task LoadUsersWithFilter() =>
            this.Users = (await this._manageUserService.GetSimpleUserInfo(this._userSearchParams)).ToGridItemList();

        public async Task SendDataToDepositoryForEditingAsync(int editedUserId)
        {
            DetailedUserInfo pageInfo = await this._manageUserService.GetDetailedUserInfo(editedUserId);
            
            await this._depository.AddOrUpdateAsync(new EditedUserDeposit()
            {
                UserEditorMode = DataEditorMode.Edit,
                UserDetails = pageInfo
            }, false);
        }

        public async Task SendDataToDepositoryForCreatingAsync()
        {
            var availableRoles = await this._manageUserService.GetSimpleRoleInfo();

            await this._depository.AddOrUpdateAsync(new EditedUserDeposit()
            {
                UserEditorMode = DataEditorMode.Create,
                UserDetails = DetailedUserInfo.MakeEmpty(availableRoles)
            }, false);
        }
        #endregion
    }
}
