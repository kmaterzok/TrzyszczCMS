using TrzyszczCMS.Core.Shared.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace TrzyszczCMS.Server.Helpers.Extensions
{
    /// <summary>
    /// Class of helpers simplifying usage of <see cref="HttpContext"/> object.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieve a unique user ID used in the database depending
        /// only on user's access token placed in the HTTP header.
        /// </summary>
        /// <param name="httpContext">HTTP context from the request used for processing data</param>
        /// <returns>Database user ID. If <c>null</c> - parsing failed or there was no specific claim storing the ID</returns>
        public static int? GetUserIdByAccessToken(this HttpContext httpContext)
        {
            var userId = httpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return !string.IsNullOrEmpty(userId) && int.TryParse(userId, out int parsedUserId) ?
                parsedUserId : null;
        }
        /// <summary>
        /// Retrieve access token from HTTP request's header.
        /// </summary>
        /// <param name="httpContext">HTTP context from the request used for processing data</param>
        /// <returns>Access token</returns>
        public static string GetAccessToken(this HttpContext httpContext) =>
            httpContext.Request.Headers[CommonConstants.HEADER_AUTHORIZATION_NAME].SingleOrDefault();
        /// <summary>
        /// Check if the user used in the <paramref name="httpContext"/> owns a role <paramref name="policyName"/>.
        /// </summary>
        /// <param name="httpContext">HTTP context from the request used for processing data</param>
        /// <param name="policyName">Name of a policy to be checked for a user</param>
        /// <returns>A verdict of the check</returns>
        public static bool HasUserPolicy(this HttpContext httpContext, string policyName) =>
            httpContext?.User?.HasClaim(i => i.Type == policyName && i.Value == true.ToString()) ?? false;
    }
}
