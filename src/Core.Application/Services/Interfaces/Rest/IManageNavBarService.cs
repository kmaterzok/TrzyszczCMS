using TrzyszczCMS.Core.Shared.Models.ManageSettings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Application.Services.Interfaces.Rest
{
    /// <summary>
    /// The interface for managing settings common for the whole system.
    /// </summary>
    public interface IManageNavBarService
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
        /// <returns>Task returning the new item</returns>
        Task<SimpleMenuItemInfo> AddMenuItem(SimpleMenuItemInfo addedItem);
        /// <summary>
        /// Delete the specific item from the menu.
        /// </summary>
        /// <param name="deletedItemId">Row ID of the deleted item</param>
        /// <returns>Task that deletes the item</returns>
        Task DeleteItem(int deletedItemId);
        /// <summary>
        /// Swap the order numbers in the items
        /// so the presentation order will be different.
        /// </summary>
        /// <param name="firstNodeId">Row ID of the first item with swapped number</param>
        /// <param name="secondNodeId">Row ID of the second item with swapped number</param>
        /// <returns>Task that swaps order numbers in the specified items</returns>
        Task SwapOrderNumbers(int firstNodeId, int secondNodeId);
    }
}
