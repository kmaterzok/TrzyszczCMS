using Core.Shared.Enums;
using System.Linq;

namespace Core.Shared.Helpers
{
    /// <summary>
    /// The helper that lets check passwords for various criteria.
    /// </summary>
    public static class PasswordSecurityHelper
    {
        /// <summary>
        /// Find reason for not allowing to change password in the backend.
        /// </summary>
        /// <param name="currentPassword">Currently used password</param>
        /// <param name="newPassword">New password to use</param>
        /// <returns>The check verdict</returns>
        public static PasswordNotChangedReason? FindReasonOfNotChangingPassword(string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                return PasswordNotChangedReason.NotAllDataProvided;
            }
            else if (!IsPasswordComplexEnough(newPassword))
            {
                return PasswordNotChangedReason.NewPasswordNotComplexEnough;
            }
            else if (currentPassword == newPassword)
            {
                return PasswordNotChangedReason.NewPasswordEqualsOldPassword;
            }
            return null;
        }
        /// <summary>
        /// Check if <paramref name="password"/> is complex enough.
        /// </summary>
        /// <param name="password">Checked password</param>
        /// <returns><paramref name="password"/> meets the complexity criteria</returns>
        private static bool IsPasswordComplexEnough(string password) =>
        (
            !string.IsNullOrEmpty(password) &&
            !string.IsNullOrWhiteSpace(password) &&
            password.Length >= 12 &&
            password.Any(i => char.IsUpper(i)) &&
            password.Any(i => char.IsLower(i)) &&
            password.Any(i => char.IsNumber(i)) &&
            password.Any(i => !char.IsUpper(i) && !char.IsLower(i) && !char.IsNumber(i))
        );
    }
}
