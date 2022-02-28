using Core.Server.Services.Interfaces.DbAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Shared.Models;
using Core.Shared.Models.Auth;
using System.Collections.Generic;
using Core.Shared.Helpers;

namespace TrzyszczCMS.Server.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        #region Fields
        private readonly IAuthDatabaseService _authService;
        #endregion

        #region Ctor
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthDatabaseService authService) : base(options, logger, encoder, clock)
        {
            this._authService = authService;
        }
        #endregion

        #region Methods
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(CommonConstants.HEADER_AUTHORIZATION_NAME))
            {
                return AuthenticateResult.NoResult();
            }
            try
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers[CommonConstants.HEADER_AUTHORIZATION_NAME]);

                var accessToken = authHeaderValue.Scheme;

                var authUserInfo = await _authService.GetAuthData(accessToken);
                if (authUserInfo == null)
                {
                    return AuthenticateResult.Fail("The token is invalid or expired.");
                }

                var claims    = ClaimsHelper.ResolveClaims(authUserInfo);
                var identity  = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket    = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch(Exception)
            {
                return AuthenticateResult.Fail("An error has occured.");
            }
        }
        #endregion
    }
}
