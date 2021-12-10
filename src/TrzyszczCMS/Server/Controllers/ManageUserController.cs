using Core.Server.Models.Enums;
using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Helpers;
using Core.Shared.Models.ManageUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TrzyszczCMS.Server.Helpers.Extensions;

namespace TrzyszczCMS.Server.Controllers
{
    [ApiController]
    [Route("ManageUser")]
    [Authorize]
    public class ManageUserController : ControllerBase
    {
        #region Fields
        private readonly IManageUserDbService _manageUserService;
        #endregion

        #region Ctor
        public ManageUserController(IManageUserDbService manageUserService)
        {
            this._manageUserService = manageUserService;
        }
        #endregion

        #region Methods
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<List<SimpleUserInfo>>> SimpleUserInfo([FromBody][NotNull] Dictionary<FilteredGridField, string> filters)
        {
            var content = await this._manageUserService.GetSimpleUserInfo(filters);
            return null != content ? Ok(content) : NotFound();
        }

        /// <summary>
        /// Delete a user from the database.
        /// </summary>
        /// <param name="pageIds">ID of the deleted user</param>
        /// <returns>Task returning HTTP response</returns>
        [HttpDelete]
        [Produces("application/json")]
        [Route("[action]/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var error = await this._manageUserService.DeleteUserAsync(userId);
            if (!error.HasValue)
            {
                return Ok();
            }
            switch (error.Value)
            {
                case DeleteRowFailReason.DeletingForbidden:
                    return Forbid();
                case DeleteRowFailReason.NotFound:
                    return NotFound();
                default:
                    throw ExceptionMaker.NotImplemented.ForHandling(error.Value, nameof(error.Value));
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{userId}")]
        public async Task<ActionResult<DetailedUserInfo>> DetailedUserInfo(int userId)
        {
            var userInfo = await this._manageUserService.GetDetailedUserInfo(userId);
            return userInfo != null ? Ok(userInfo) : NotFound();
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<List<SimpleRoleInfo>>> SimpleRoleInfo() =>
            Ok(await this._manageUserService.GetSimpleRoleInfo());

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{username}")]
        public async Task<ActionResult> UserNameExists(string username) =>
            await this._manageUserService.UserNameExists(username) ? Ok() : NotFound();

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<string>> AddUser([FromBody][NotNull] DetailedUserInfo request) =>
            (await this._manageUserService.AddUserAsync(request)).GetValue(out string password, out Tuple<bool> _) ?
                Ok(password) : Conflict();

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult> UpdateUser([FromBody][NotNull] DetailedUserInfo request) =>
            await this._manageUserService.UpdateUserAsync(request) ? Ok() : Conflict();



        [HttpDelete]
        [Produces("application/json")]
        [Route("[action]/{tokenId}")]
        public async Task<ActionResult> RevokeToken(int tokenId)
        {
            var userId = HttpContext.GetUserIdByAccessToken();
            var usersCurrentToken = HttpContext.GetAccessToken();

            if (!userId.HasValue || string.IsNullOrEmpty(usersCurrentToken))
            {
                return Forbid();
            }

            var error = await this._manageUserService.RevokeTokenAsync(tokenId, userId.Value, usersCurrentToken);
            if (!error.HasValue)
            {
                return Ok();
            }
            switch (error.Value)
            {
                case DeleteRowFailReason.DeletingForbidden:
                    return Forbid();
                case DeleteRowFailReason.NotFound:
                    return NotFound();
                case DeleteRowFailReason.DeletingOwnStuff:
                    return Conflict();
                default:
                    throw ExceptionMaker.NotImplemented.ForHandling(error.Value, nameof(error.Value));
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<List<SimpleRoleInfo>>> OwnSimpleTokenInfo()
        {
            var userId = HttpContext.GetUserIdByAccessToken();
            var usersCurrentToken = HttpContext.GetAccessToken();

            if (!userId.HasValue || string.IsNullOrEmpty(usersCurrentToken))
            {
                return Forbid();
            }
            return Ok(await this._manageUserService.OwnSimpleTokenInfoAsync(userId.Value, usersCurrentToken));
        }

        #endregion
    }
}
