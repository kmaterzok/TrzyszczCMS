using TrzyszczCMS.Core.Shared.Models;
using TrzyszczCMS.Core.Shared.Models.ManageSettings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Server.Services.Interfaces.DbAccess.Modify
{
    /// <summary>
    /// The interface for managing website settings.
    /// </summary>
    public interface IManageNavBarDbService
    {
        /// <summary>
        /// Get a list of items in the menu, assigned to a specific node.
        /// </summary>
        /// <param name="parentItemId">Row Id of the item that holds the desired ones</param>
        /// <returns>Task returning a list of desired items</returns>
        Task<List<SimpleMenuItemInfo>> GetSimpleMenuItemInfos(int? parentItemId);
        /// <summary>
        /// Add a new item to the menu.
        /// </summary>
        /// <param name="addedItem">Data of the added item</param>
        /// <returns>Task executing the adding. Returns a new item or error info</returns>
        Task<Result<SimpleMenuItemInfo, object>> AddMenuItem(SimpleMenuItemInfo addedItem);
        /// <summary>
        /// Delete the specific item from the menu.
        /// </summary>
        /// <param name="deletedItemId">Row ID of the deleted item</param>
        /// <returns>Task returning if the deleted element was found and deleted</returns>
        Task<bool> DeleteItem(int deletedItemId);
        /// <summary>
        /// Swap the order numbers in the items
        /// so the presentation order will be different.
        /// </summary>
        /// <param name="firstNodeId">Row ID of the first item with swapped number</param>
        /// <param name="secondNodeId">Row ID of the second item with swapped number</param>
        /// <returns>Task that swaps order numbers in the specified items. Returns if specified data was found and swapped</returns>
        Task<bool> SwapOrderNumbers(int firstNodeId, int secondNodeId);
    }
}
