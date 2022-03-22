using TrzyszczCMS.Core.Shared.Models.Auth;
using System.Collections.Generic;
using System.Security.Claims;

namespace TrzyszczCMS.Core.Shared.Helpers
{
    /// <summary>
    /// It eases processeing data that relies on <see cref="Claim"/>
    /// </summary>
    public static class ClaimsHelper
    {
        /// <summary>
        /// Get enumeration of all <see cref="Claim"/> characteristics that owns the authenticated user.
        /// </summary>
        /// <param name="user">Authenticated user object</param>
        /// <returns>Enumeration of all owned characteristics</returns>
        public static IEnumerable<Claim> ResolveClaims(AuthUserInfo user)
        {
            yield return new Claim(ClaimTypes.Name, user.Username);
            yield return new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString());

            foreach (var policy in user.AssignedPoliciesNames)
            {
                yield return new Claim(policy, true.ToString());
            }
        }
    }
}
