namespace TrzyszczCMS.Core.Application.Helpers
{
    /// <summary>
    /// The class of validation methods.
    /// </summary>
    public static class SignInValidationHelper
    {
        /// <summary>
        /// Check if the enetred credentials meet basic criteria for sign in.
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        /// <param name="error">An error that might happen</param>
        /// <returns>It says if the cccredentials are OK</returns>
        public static bool CheckCredentials(string username, string password, out string error)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                error = "No username or password entered.";
                return false;
            }
            error = string.Empty;
            return true;
        }
    }
}
