using Core.Application.Services.Interfaces;
using Core.Application.Services.Interfaces.Rest;
using TrzyszczCMS.Client.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using TrzyszczCMS.Client.Other;
using Microsoft.AspNetCore.Authorization;
using TrzyszczCMS.Client.Data.Enums;
using Core.Shared.Helpers;
using System;
using DAL.Shared.Data;

namespace TrzyszczCMS.Client.Services.Implementation
{
    public class AuthService : IAuthService
    {
        #region Fields
        /// <summary>
        /// Reference on the service that manages tokens.
        /// </summary>
        private readonly ITokenService _tokenService;
        /// <summary>
        /// Used for verifying credentials and getting auth data.
        /// </summary>
        private readonly IRestAuthService _authRestService;
        /// <summary>
        /// Service used for auhtorising user and checking policies.
        /// </summary>
        private readonly IAuthorizationService _authorizationService;
        /// <summary>
        /// Application's provider of authentication state. 
        /// </summary>
        private readonly ApplicationAuthenticationStateProvider _authStateProvider;
        #endregion

        #region Ctor
        public AuthService(ITokenService tokenService, IRestAuthService authRestService, IAuthorizationService authorizationService, AuthenticationStateProvider authStateProvider)
        {
            this._tokenService = tokenService;
            this._authRestService = authRestService;
            this._authorizationService = authorizationService;
            this._authStateProvider = (ApplicationAuthenticationStateProvider)authStateProvider;
        }
        #endregion

        #region Methods
        public async Task<bool> AuthenticateWithCredentialsAsync(string username, string password, bool remember)
        {
            var authUserInfo = await this._authRestService.GenerateAuthData(username, password, remember);
            if (null != authUserInfo)
            {
                await this._tokenService.SetTokenAsync(authUserInfo.AccessToken);
                await this._authStateProvider.NotifyAuthenticationStateChange();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task RevokeAuthenticationAsync()
        {
            await this._authRestService.RevokeToken(await this._tokenService.GetTokenAsync());
            await this._tokenService.RevokeTokenAsync();
            await this._authStateProvider.NotifyAuthenticationStateChange();
        }
        public async Task<bool> IsAuthenticated() =>
            (await this._authStateProvider.GetAuthenticationStateAsync())?.User?.Identity?.IsAuthenticated ?? false;

        public async Task<bool> HasClearanceAsync(PolicyClearance clearance) => clearance switch
        {
            PolicyClearance.AccessNavBarSettings => await this.HasAllPoliciesAsync(UserPolicies.MANAGE_NAVIGATION_BAR),

            _ => throw ExceptionMaker.NotImplemented.ForHandling(clearance, nameof(clearance))
        };
        
        #endregion

        #region Helper methods
        private async Task<bool> HasAllPoliciesAsync(params string[] policyNames) =>
            await HasPoliciesAsync(EnumerableItemsComplianceCheckMethod.All, policyNames);

        private async Task<bool> HasAnyPolicyAsync(params string[] policyNames) =>
            await HasPoliciesAsync(EnumerableItemsComplianceCheckMethod.Any, policyNames);

        public async Task<bool> HasPoliciesAsync(EnumerableItemsComplianceCheckMethod method, params string[] policyNames)
        {
            if (!await this.IsAuthenticated())
            {
                return false;
            }

            var user = (await this._authStateProvider.GetAuthenticationStateAsync()).User;

            bool policiesMatch;
            Func<bool, bool, bool> flagApplier;

            switch (method)
            {
                case EnumerableItemsComplianceCheckMethod.All:
                    policiesMatch = true;
                    flagApplier = (i1, i2) => i1 && i2;
                    break;

                case EnumerableItemsComplianceCheckMethod.Any:
                    policiesMatch = false;
                    flagApplier = (i1, i2) => i1 || i2;
                    break;

                default:
                    throw ExceptionMaker.Argument.Invalid(method, nameof(method));
            }

            foreach (var policyName in policyNames)
            {
                bool hasPolicy = (await this._authorizationService.AuthorizeAsync(user, policyName)).Succeeded;
                policiesMatch = flagApplier.Invoke(policiesMatch, hasPolicy);
            }
            return policiesMatch;
        }
        #endregion
    }
}
