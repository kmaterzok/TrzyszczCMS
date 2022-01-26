namespace Core.Shared.Models.Rest.Requests.ManageUsers
{
    /// <summary>
    /// The request of the password change for the currently signed in user.
    /// </summary>
    public class ChangeOwnPasswordRequest
    {
        /// <summary>
        /// The password that is currently in use.
        /// </summary>
        public string CurrentPassword { get; set; }
        /// <summary>
        /// The new password for the currently signed in user.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
