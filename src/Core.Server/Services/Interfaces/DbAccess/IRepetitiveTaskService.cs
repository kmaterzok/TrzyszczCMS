using System.Threading.Tasks;

namespace Core.Server.Services.Interfaces.DbAccess
{
    /// <summary>
    /// Interface for service that repetitively
    /// executes tasks in specified conditions.
    /// </summary>
    /// <remarks>Important: Always instantiate as singleton</remarks>
    public interface IRepetitiveTaskService
    {
        /// <summary>
        /// Start a Timer that revokes access token and revoke currently expired ones
        /// If all tokens are revoked then the Timer is disposed.
        /// </summary>
        /// <returns>Task executing the revocation</returns>
        Task StartRevokingTokensAsync();
    }
}
