using TrzyszczCMS.Core.Application.Services.Interfaces.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;
using TrzyszczCMS.Client.Views.Administering;

namespace TrzyszczCMS.Client.ViewModels.Administering
{
    /// <summary>
    /// The viewmodel used for <see cref="ChangeOwnPassword"/> instances.
    /// </summary>
    public class ChangeOwnPasswordViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// The service used for password change.
        /// </summary>
        private readonly IManageUserService _manageUserService;
        #endregion

        #region Properties
        private string _errorMessage;
        /// <summary>
        /// Message informing about the error that happened.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value, nameof(ErrorMessage));
        }

        private string _oldPassword;
        /// <summary>
        /// Currently user password of the user.
        /// </summary>
        public string OldPassword
        {
            get => _oldPassword;
            set { Set(ref _oldPassword, value, nameof(OldPassword)); this.ErrorMessage = string.Empty; }
        }

        private string _newPassword;
        /// <summary>
        /// New user password for the user.
        /// </summary>
        public string NewPassword
        {
            get => _newPassword;
            set { Set(ref _newPassword, value, nameof(NewPassword)); this.ErrorMessage = string.Empty; }
        }

        private string _newPasswordRepeated;
        /// <summary>
        /// Repeating of the new user password for the user.
        /// </summary>
        public string NewPasswordRepeated
        {
            get => _newPasswordRepeated;
            set { Set(ref _newPasswordRepeated, value, nameof(NewPasswordRepeated)); this.ErrorMessage = string.Empty; }
        }

        /// <summary>
        /// Fired when the password change was successful.
        /// </summary>
        public EventHandler OnAfterSuccess { get; set; }
        #endregion

        #region Ctor
        public ChangeOwnPasswordViewModel(IManageUserService manageUserService) =>
            this._manageUserService = manageUserService;
        #endregion

        #region Methods
        /// <summary>
        /// Set empty data for changing the password.
        /// </summary>
        public void ClearData()
        {
            this.OldPassword         = string.Empty;
            this.NewPassword         = string.Empty;
            this.NewPasswordRepeated = string.Empty;
            this.ErrorMessage        = string.Empty;
        }
        /// <summary>
        /// Change the password for the signed in user.
        /// </summary>
        /// <returns>Was execution successful</returns>
        public async Task<bool> ChangePassword()
        {
            var localVerdict = PasswordChangeViewHelper.CheckPasswordsForChangeBeforeSending(this.OldPassword, this.NewPassword, this.NewPasswordRepeated);
            this.ErrorMessage = localVerdict.HasValue ?
                localVerdict.Value.GetTranslation() :
                string.Empty;
            
            if (localVerdict.HasValue)
            {
                return false;
            }

            var remoteVerdict = await this._manageUserService.ChangeSignedInUserPassword(this.OldPassword, this.NewPassword);
            this.ErrorMessage = remoteVerdict.HasValue ?
                remoteVerdict.Value.GetTranslation() :
                string.Empty;

            bool success = !localVerdict.HasValue && !remoteVerdict.HasValue;

            if (success)
            {
                this.OnAfterSuccess?.Invoke(this, EventArgs.Empty);
            }
            return success;
        }
        #endregion
    }
}
