using Core.Application.Enums;
using Core.Application.Models.Deposits;
using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Models.ManageUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering.Edit
{
    public class EditedUserDepositViewModel : ViewModelBase
    {
        #region Fields
        private readonly IManageUserService _manageUserService;
        private string _oldUserName;
        #endregion

        #region Properties :: Work & modes
        private DataEditorMode _userEditorMode;
        /// <summary>
        /// Work mode of editor
        /// </summary>
        public DataEditorMode UserEditorMode
        {
            get => _userEditorMode;
            set => Set(ref _userEditorMode, value, nameof(UserEditorMode));
        }
        #endregion

        #region Properties :: Validation messages
        private string _userNameValidationMessage;
        /// <summary>
        /// The additional message about login validation.
        /// </summary>
        public string UserNameValidationMessage
        {
            get => _userNameValidationMessage;
            set => Set(ref _userNameValidationMessage, value, nameof(UserNameValidationMessage));
        }

        private string _assignedRoleValidationMessage;
        /// <summary>
        /// The additional message about assigned role validation.
        /// </summary>
        public string AssignedRoleValidationMessage
        {
            get => _assignedRoleValidationMessage;
            set => Set(ref _assignedRoleValidationMessage, value, nameof(AssignedRoleValidationMessage));
        }
        #endregion

        #region Properties :: Edited data
        /// <summary>
        /// Database row ID of the user
        /// </summary>
        public int IdOfUser { get; set; }

        private string _userName;
        /// <summary>
        /// Login / user name of the user.
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => Set(ref _userName, value, nameof(UserName));
        }

        private string _description;
        /// <summary>
        /// Additional description.
        /// </summary>
        public string Description
        {
            get => _description;
            set => Set(ref _description, value, nameof(Description));
        }

        private int _assignedRoleId;
        /// <summary>
        /// Row ID of the role assigned to this user.
        /// </summary>
        public int AssignedRoleId
        {
            get => _assignedRoleId;
            set => Set(ref _assignedRoleId, value, nameof(AssignedRoleId));
        }

        private List<SimpleRoleInfo> _availableRoles;
        /// <summary>
        /// Roles available to be assigned to this user.
        /// </summary>
        public List<SimpleRoleInfo> AvailableRoles
        {
            get => _availableRoles;
            set => Set(ref _availableRoles, value, nameof(AvailableRoles));
        }
        #endregion

        #region Ctor
        public EditedUserDepositViewModel(EditedUserDeposit deposit, IManageUserService manageUserService)
        {
            this._manageUserService = manageUserService;
            this.UserEditorMode     = deposit.UserEditorMode;

            this.IdOfUser           = deposit.UserDetails.Id;
            this._oldUserName       = deposit.UserDetails.UserName;
            this.UserName           = deposit.UserDetails.UserName;
            this.Description        = deposit.UserDetails.Description;
            this.AssignedRoleId     = deposit.UserDetails.AssignedRoleId;
            this.AvailableRoles     = deposit.UserDetails.AvailableRoles;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check validity of the data.
        /// </summary>
        /// <returns>Is data valid</returns>
        public async Task<bool> ValidateAndInformAsync()
        {
            this.ClearReachableMessages();
            bool valid = true;
            bool oldAndNewNameAreEqual = this._oldUserName == this.UserName;

            // *** UserName ***
            if (oldAndNewNameAreEqual)
            {
                // Do nothing
            }
            this.UserNameValidationMessage = ValidationApplier.CheckRequired(this.UserName, ref valid);
            if (!valid)
            {
                // Do nothing
            }
            else if (!oldAndNewNameAreEqual)
            {
                if ((await this._manageUserService.UserNameExists(this.UserName)).GetValue(out Tuple<bool> success, out string error))
                {
                    if (success.Item1)
                    {
                        this.UserNameValidationMessage = "Already in use. Change it.";
                    }
                    valid = !success.Item1;
                }
                else
                {
                    this.UserNameValidationMessage = error == "PatternMismatch" ?
                        "Forbidden characters. Change it." :
                        "Incorrect data.";

                    valid = false;
                }
            }
            // *** ***

            // *** AssignedRoleId ***
            if (this.AssignedRoleId == 0)
            {
                this.AssignedRoleValidationMessage = "Choose a role.";
                valid = false;
            }
            else if (this.AvailableRoles.All(i => i.Id != this.AssignedRoleId))
            {
                this.AssignedRoleValidationMessage = "Incorrect assign.";
                valid = false;
            }
            // *** ***

            return valid;
        }

        /// <summary>
        /// Create and get a detailed user info.
        /// </summary>
        /// <returns>Modifiable and manageable information about user</returns>
        public DetailedUserInfo GetDetailedUserInfo() => new DetailedUserInfo()
        {
            Id             = this.IdOfUser,
            UserName       = this.UserName,
            Description    = this.Description,
            AssignedRoleId = this.AssignedRoleId,
            AvailableRoles = null
        };
        #endregion

        #region Helper methods
        /// <summary>
        /// Clear all the messages that were set programatically within this class.
        /// </summary>
        private void ClearReachableMessages()
        {
            this.AssignedRoleValidationMessage = string.Empty;
            this.UserNameValidationMessage     = string.Empty;
        }
        #endregion
    }
}
