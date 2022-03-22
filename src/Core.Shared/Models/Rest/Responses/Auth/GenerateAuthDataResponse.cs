using TrzyszczCMS.Core.Shared.Models.Auth;

namespace TrzyszczCMS.Core.Shared.Models.Rest.Responses.Auth
{
    /// <summary>
    /// The response class for <see cref="Requests.GenerateAuthDataRequest"/>
    /// with client-side authentication and authorisation data.
    /// </summary>
    public class GenerateAuthDataResponse
    {
        /// <summary>
        /// Data returned for user that has signed in with concrete credentials.
        /// </summary>
        public AuthUserInfo AuthUserInfo { get; set; }
    }
}
