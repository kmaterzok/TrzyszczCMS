﻿using Core.Server.Services.Interfaces.DbAccess;
using Core.Shared.Models.Rest.Requests.Auth;
using Core.Shared.Models.Rest.Responses.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrzyszczCMS.Server.Controllers
{
    /// <summary>
    /// The controller used for authorisation
    /// of users that try to sign in to the backend.
    /// </summary>
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        #region Fields
        /// <summary>
        /// Service handling basic authorisation and authentication with database usage.
        /// </summary>
        private readonly IAuthDatabaseService _authService;
        #endregion

        #region Ctor
        public AuthController(IAuthDatabaseService authService)
        {
            this._authService = authService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate client-size authorisation data.
        /// </summary>
        /// <param name="request">Basic credentials of signing in user</param>
        /// <returns>Auth data</returns>
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<GenerateAuthDataResponse>> GenerateData([FromBody] GenerateAuthDataRequest request)
        {
            var result = await this._authService.GenerateAuthData(request.Username, request.Password, request.RememberMe);
            return (result != null) ?
                Ok(new GenerateAuthDataResponse() { AuthUserInfo = result }) :
                Forbid();
        }
        /// <summary>
        /// Get client-side authorisation data for already created access token.
        /// </summary>
        /// <returns>Auth data</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{token}")]
        public async Task<ActionResult<GenerateAuthDataResponse>> GetData(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Forbid();
            }
            var result = await this._authService.GetAuthData(token);
            return (result != null) ?
                Ok(new GenerateAuthDataResponse() { AuthUserInfo = result }) :
                Forbid();
        }
        #endregion
    }
}