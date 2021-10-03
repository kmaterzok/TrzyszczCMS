using Core.Shared.Models.Auth;

namespace Core.Shared.Models.Rest.Responses.Auth
{
    /// <summary>
    /// The response class with client-side authentication and authorisation data.
    /// </summary>
    public class GetAuthDataResponse
    {
        /// <summary>
        /// Data returned for user that has signed in with concrete credentials.
        /// </summary>
        public AuthUserInfo AuthUserInfo { get; set; }
    }
}
