using ApplicationCore.Models.Auth;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Interfaces.DbAccess
{
    /// <summary>
    /// It provides basic authentication and authorisation checks and returns.
    /// </summary>
    public interface IAuthDatabaseService
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
    }
}
