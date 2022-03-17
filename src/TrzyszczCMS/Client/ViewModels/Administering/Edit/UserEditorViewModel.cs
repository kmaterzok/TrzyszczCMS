using Core.Application.Enums;
using Core.Application.Models.Deposits;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Helpers;
using System;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class UserEditorViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDataDepository _depository;
        private readonly IManageUserService _manageUserService;
        private readonly IAuthService _authService;
        #endregion

        #region Properties
        /// <summary>
        /// Fired when there is a need to exit the current view.
        /// </summary>
        public Action OnExitingView { get; set; }

        private bool _isManagementInfoProvided;
        /// <summary>
        /// It says whether there is data allowing to manage a page.
        /// </summary>
        public bool IsManagementInfoProvided
        {
            get => _isManagementInfoProvided;
            set => Set(ref _isManagementInfoProvided, value, nameof(IsManagementInfoProvided));
        }

        private bool _arePermissionsForSpecifiedManagementProvided;
        /// <summary>
        /// It says whether the signed in user has sufficient policies
        /// assigned to itself so it is able to manage user data.
        /// </summary>
        public  bool ArePermissionsForSpecifiedManagementProvided
        {
            get => _arePermissionsForSpecifiedManagementProvided;
            set => Set(ref _arePermissionsForSpecifiedManagementProvided, value, nameof(ArePermissionsForSpecifiedManagementProvided));
        }

        private EditedUserDepositViewModel _editedUserDepositVM;
        /// <summary>
        /// Remapped instance of <see cref="EditedUserDepositVM"/> for editing purposes.
        /// </summary>
        public EditedUserDepositViewModel EditedUserDepositVM
        {
            get => _editedUserDepositVM;
            set => Set(ref _editedUserDepositVM, value, nameof(EditedUserDepositVM));
        }

        private string _generatedPassword;
        /// <summary>
        /// The password for a newly created user generated on the server.
        /// </summary>
        public string GeneratedPassword
        {
            get => _generatedPassword;
            set => Set(ref _generatedPassword, value, nameof(GeneratedPassword));
        }
        #endregion

        #region Init
        public UserEditorViewModel(IDataDepository depository, IManageUserService manageUserService, IAuthService authService)
        {
            this.GeneratedPassword = null;
            this._depository = depository;
            this._manageUserService = manageUserService;
            this._authService = authService;
        }
        public async Task LoadDataFromDeposit()
        {
            var gotDeposit = await this._depository.GetAsync<EditedUserDeposit>();

            bool managingPossible = gotDeposit != null;
            if (managingPossible)
            {
                EditedUserDepositVM = new EditedUserDepositViewModel(gotDeposit, this._manageUserService);
                EditedUserDepositVM.PropertyChanged += (_, e) => this.NotifyPropertyChanged(e.PropertyName);
                await this.ResolveManagementClearancesAsync(gotDeposit.UserEditorMode);
            }
            IsManagementInfoProvided = managingPossible;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Apply all changes of the user.
        /// </summary>
        /// <returns>Task executing the apply</returns>
        public async Task ApplyChanges()
        {
            if (await this.ValidateAndInform())
            {
                switch (this.EditedUserDepositVM.UserEditorMode)
                {
                    case DataEditorMode.Create:
                        this.GeneratedPassword = await this._manageUserService.AddUser(this.EditedUserDepositVM.GetDetailedUserInfo());
                        break;

                    case DataEditorMode.Edit:
                        await this._manageUserService.UpdateUser(this.EditedUserDepositVM.GetDetailedUserInfo());
                        break;

                    default:
                        throw new InvalidOperationException($"{this.EditedUserDepositVM.UserEditorMode} is not allowed.");
                }
                await this._depository.RemoveAsync<EditedUserDeposit>();

                if (this.EditedUserDepositVM.UserEditorMode != DataEditorMode.Create)
                {
                    ExitView();
                }
            }
        }

        public void ExitView()
        {
            this.GeneratedPassword = null;
            this.OnExitingView?.Invoke();
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Check validity of the entered data and inform a user
        /// by setting proper messages.
        /// </summary>
        /// <returns>Task returning if data is valid</returns>
        private async Task<bool> ValidateAndInform() =>
            await this.EditedUserDepositVM.ValidateAndInformAsync();

        /// <summary>
        /// Check if there are any policies that block creating or editing the user data
        /// depending on the provided work settings.
        /// </summary>
        /// <param name="editorMode">Data editor work mode</param>
        /// <returns>The executing task</returns>
        private async Task ResolveManagementClearancesAsync(DataEditorMode editorMode)
        {
            var expectedClearance = editorMode.GetClearanceOfUserManagement();
            this.ArePermissionsForSpecifiedManagementProvided = await this._authService.HasClearanceAsync(expectedClearance);
        }
        #endregion
    }
}
