using TrzyszczCMS.Core.Shared.Models.Auth;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Application.Services.Interfaces.Rest
{
    /// <summary>
    /// It provides basic authentication and authorisation
    /// checks and returns by using connection with REST API.
    /// </summary>
    public interface IRestAuthService
    {
        /// <summary>
        /// Return data about authenticated user by <see cref="accessToken"/>.
        /// </summary>
        /// <param name="accessToken">String authenticating a user for usage.</param>
        /// <returns>Data about authorised user. <c>null</c> if token invalid or expired.</returns>
        Task<AuthUserInfo> GetAuthData(string accessToken);
        /// <summary>
        /// Return data about authenticateduser by basic credentials and authenticate user.
        /// </summary>
        /// <param name="username">User's login</param>
        /// <param name="password">Password of the user</param>
        /// <param name="remember">It says if the user wants to be signed in for a long time</param>
        /// <returns>Data about authorised user. <c>null</c> if credentials invalid.</returns>
        Task<AuthUserInfo> GenerateAuthData(string username, string password, bool remember);
        /// <summary>
        /// Revoke token in the backend - remove from the database.
        /// </summary>
        /// <param name="accessToken">Revoked access token</param>
        /// <returns></returns>
        Task RevokeToken(string accessToken);
    }
}
