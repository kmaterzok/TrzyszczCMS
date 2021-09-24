using ApplicationCore.Models.Auth;
using ApplicationCore.Services.Interfaces.DbAccess;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TrzyszczCMS.Data;
using TrzyszczCMS.Services.Interfaces;

namespace TrzyszczCMS.Other
{
    /// <summary>
    /// The provider of authorisation status for the application.
    /// </summary>
    public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region Fields
        /// <summary>
        /// Used for managing token for authorisation.
        /// </summary>
        private readonly ITokenService _tokenService;
        /// <summary>
        /// Used for verifying credentials and getting auth data.
        /// </summary>
        private readonly IAuthDatabaseService _authDbService;
        #endregion

        #region Ctor
        /// <summary>
        /// Create an instance of <see cref="AuthenticationStateProvider"/> compliant object.
        /// </summary>
        public ApplicationAuthenticationStateProvider(ITokenService tokenService, IAuthDatabaseService authDbService)
        {
            this._tokenService = tokenService;
            this._authDbService = authDbService;
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Get enumeration of all <see cref="Claim"/> characteristics that owns the authenticated user.
        /// </summary>
        /// <param name="user">Authenticated user object</param>
        /// <returns>Enumeration of all owned characteristics</returns>
        private static IEnumerable<Claim> ResolveClaims(AuthUserInfo user)
        {
            yield return new Claim(ClaimTypes.Name, user.Username);

            foreach(var policy in user.AssignedPoliciesNames)
            {
                yield return new Claim(policy, true.ToString());
            }
        }
        #endregion

        #region Protected methods
        protected ClaimsIdentity GetClaimsIdentity(AuthUserInfo user)
        {
            var claimsIdentity = (user != null && user.Username != null) ?
                new ClaimsIdentity(ResolveClaims(user), Constants.APPLICATION_AUTH_TYPE_NAME) :
                new ClaimsIdentity();
            
            return claimsIdentity;
        }
        #endregion

        #region Methods
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string accessToken = await _tokenService.GetTokenAsync();

            ClaimsIdentity identity;

            if (string.IsNullOrEmpty(accessToken))
            {
                identity = new ClaimsIdentity();
            }
            else
            {
                AuthUserInfo user = await this._authDbService.GetAuthData(accessToken);
                if (user == null)
                {
                    await this._tokenService.RevokeTokenAsync();
                }
                identity = GetClaimsIdentity(user);
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        #endregion
    }
}
