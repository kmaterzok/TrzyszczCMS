using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers;

namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// The helper that provides methods used in the view of password change.
    /// </summary>
    public static class PasswordChangeViewHelper
    {
        /// <summary>
        /// Check if the password can be changed and check
        /// if other data provided in the correspondent view are valid.
        /// </summary>
        /// <param name="oldPassword">Currently used password</param>
        /// <param name="newPassword">New password for the user</param>
        /// <param name="newPasswordRepeated">Repeated password for assuring if the user remembers the password correctly</param>
        /// <returns>Fail reason of not changing the password</returns>
        public static PasswordNotChangedReason? CheckPasswordsForChangeBeforeSending(string oldPassword, string newPassword, string newPasswordRepeated)
        {
            if (string.IsNullOrEmpty(newPasswordRepeated))
            {
                return PasswordNotChangedReason.NotAllDataProvided;
            }
            var verdict = PasswordSecurityHelper.FindReasonOfNotChangingPassword(oldPassword, newPassword);

            if (!verdict.HasValue)
            {
                if (oldPassword == newPassword)
                {
                    return PasswordNotChangedReason.NewPasswordEqualsOldPassword;
                }
                else if (newPassword != newPasswordRepeated)
                {
                    return PasswordNotChangedReason.RepeatedPasswordInvalid;
                }
            }
            return verdict;
        }
    }
}
