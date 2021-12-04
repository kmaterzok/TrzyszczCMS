using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Enums;
using Core.Shared.Models.ManageUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

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
        public async Task<ActionResult> DeletePages(int userId) =>
            await this._manageUserService.DeleteUserAsync(userId) ? Ok() : NotFound();

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
        #endregion
    }
}
