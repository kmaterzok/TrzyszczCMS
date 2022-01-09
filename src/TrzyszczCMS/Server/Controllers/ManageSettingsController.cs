using Core.Server.Services.Interfaces.DbAccess.Modify;
using Core.Shared.Models.ManageSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrzyszczCMS.Server.Helpers.Extensions;

namespace TrzyszczCMS.Server.Controllers
{
    [ApiController]
    [Route("ManageSettings")]
    [Authorize]
    public class ManageSettingsController : ControllerBase
    {
        #region Fields
        private readonly IManageSettingsDbService _manageSettingsService;
        #endregion

        #region Ctor
        public ManageSettingsController(IManageSettingsDbService manageSettingsService) =>
            this._manageSettingsService = manageSettingsService;
        #endregion

        #region Methods
        [HttpGet]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<List<SimpleMenuItemInfo>>> SimpleMenuItemInfos() =>
            await this.GetSimpleMenuItemInfosAsync(null);

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{parentItemId}")]
        public async Task<ActionResult<List<SimpleMenuItemInfo>>> SimpleMenuItemInfos(int parentItemId) =>
            await this.GetSimpleMenuItemInfosAsync(parentItemId);

        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult<SimpleMenuItemInfo>> AddMenuItem([FromBody] SimpleMenuItemInfo addedItem) =>
            (await this._manageSettingsService.AddMenuItem(addedItem)).GetValue(out SimpleMenuItemInfo menuItem, out _) ?
                this.ObjectCreated(menuItem) :
                Conflict("A new item cannot have a parent node that has another parent node. A new item cannot be named as ..");

        [HttpDelete]
        [Produces("application/json")]
        [Route("[action]/{itemId}")]
        public async Task<ActionResult> DeleteMenuItem(int itemId) =>
            (await this._manageSettingsService.DeleteItem(itemId)) ? Ok() : NotFound();

        [HttpGet]
        [Produces("application/json")]
        [Route("[action]/{firstNodeId}/{secondNodeId}")]
        public async Task<ActionResult> SwapOrderNumbers(int firstNodeId, int secondNodeId) =>
            (await this._manageSettingsService.SwapOrderNumbers(firstNodeId, secondNodeId)) ? Ok() : NotFound();
        #endregion

        #region Helper methods
        private async Task<ActionResult<List<SimpleMenuItemInfo>>> GetSimpleMenuItemInfosAsync(int? parentItemId)
        {
            var pageDetails = await this._manageSettingsService.GetSimpleMenuItemInfos(parentItemId);
            return null != pageDetails ? Ok(pageDetails) : NotFound();
        }
        #endregion
    }
}
