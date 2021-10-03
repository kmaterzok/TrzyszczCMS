using System.Threading.Tasks;

namespace Core.Application.Services.Interfaces
{
    /// <summary>
    /// Interface for managing of access token in the browser.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Set token in Local Storage.
        /// </summary>
        /// <param name="accessToken">Access token user for authorisation</param>
        /// <returns></returns>
        public Task SetTokenAsync(string accessToken);
        /// <summary>
        /// Get access token stored in Local Storage.
        /// </summary>
        /// <returns>Access token user for authorisation</returns>
        /// <returns></returns>
        public Task<string> GetTokenAsync();
        /// <summary>
        /// Delete token from Local Storage and revoke it in the database.
        /// </summary>
        /// <returns></returns>
        public Task RevokeTokenAsync();
    }
}
