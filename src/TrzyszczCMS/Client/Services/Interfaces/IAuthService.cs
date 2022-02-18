using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Services.Interfaces
{
    /// <summary>
    /// The interface for managing
    /// authentication and authorisation of users.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate user with supplied credentials. It saves the access token to the LocalStorage.
        /// To apply changes in the app the <see cref="AuthenticationStateProvider"/> object must provide a proper authentication data.
        /// </summary>
        /// <param name="username">Username of signing in user</param>
        /// <param name="password">Password of signing in user</param>
        /// <param name="remember">It says if the user wants to be signed in for a long time</param>
        /// <returns>Are delivered credentials valid</returns>
        public Task<bool> AuthenticateWithCredentialsAsync(string username, string password, bool remember);
        /// <summary>
        /// Sign out currently signed in user.
        /// </summary>
        /// <returns>Task executing</returns>
        public Task RevokeAuthenticationAsync();
        /// <summary>
        /// Is the user authenticated (signed in) in the backend.
        /// </summary>
        /// <returns>Look at the summary.</returns>
        Task<bool> IsAuthenticated();
    }
}
