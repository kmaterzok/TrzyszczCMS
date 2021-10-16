using Core.Shared.Models;
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

            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int parsedUserId))
            {
                return parsedUserId;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Retrieve access token from HTTP request's header.
        /// </summary>
        /// <param name="httpContext">HTTP context from the request used for processing data</param>
        /// <returns>Access token</returns>
        public static string GetAccessToken(this HttpContext httpContext)
        {
            return httpContext.Request.Headers[CommonConstants.HEADER_AUTHORIZATION_NAME].SingleOrDefault();
        }
    }
}
