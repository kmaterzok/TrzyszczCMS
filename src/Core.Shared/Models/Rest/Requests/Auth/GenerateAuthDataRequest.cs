namespace Core.Shared.Models.Rest.Requests.Auth
{
    /// <summary>
    /// The request sent with signing in user's credentials.
    /// </summary>
    public class GenerateAuthDataRequest
    {
        /// <summary>
        /// Signing in user's name
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password of the signing in user.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The flag indicating whether the token designated for
        /// the user's session has to be valid for a very long time.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
