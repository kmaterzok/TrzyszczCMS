using Core.Application.Services.Interfaces.Rest;
using Core.Shared.Models.ManageSettings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model;
using TrzyszczCMS.Client.Data.Model.Extensions;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.Administering
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IManageSettingsService _manageSettingsService;
        #endregion

        #region Ctor
        public SettingsViewModel(IManageSettingsService manageSettingsService)
        {
            this._manageSettingsService = manageSettingsService;
            this.MenuItems = new List<GridItem<SimpleMenuItemInfo>>();
            this.CurrentMenuItemsParentNodeId = null;
        }
        #endregion

        #region Properties
        private List<GridItem<SimpleMenuItemInfo>> _menuItems;
        /// <summary>
        /// All menu items currently displayed in the grid.
        /// </summary>
        public List<GridItem<SimpleMenuItemInfo>> MenuItems
        {
            get => _menuItems;
            set => Set(ref _menuItems, value, nameof(MenuItems));
        }
        /// <summary>
        /// The row ID of the parent node that holds all the loaded menu items.
        /// </summary>
        public int? CurrentMenuItemsParentNodeId { get; set; }
        #endregion

        #region Methods
        public async Task DeleteMenuItemAsync(int itemId)
        {
            await this._manageSettingsService.DeleteItem(itemId);
            this.MenuItems.Remove(this.MenuItems.Single(i => i.Data.Id == itemId));
            this.NotifyPropertyChanged(nameof(MenuItems));
        }

        public async Task AddMenuItemAsync(SimpleMenuItemInfo addedItem)
        {
            var newItem = await this._manageSettingsService.AddMenuItem(addedItem);
            this.MenuItems.AddAndPack(newItem);
            this.NotifyPropertyChanged(nameof(MenuItems));
        }

        public async Task LoadMenuItemsAsync() =>
            this.MenuItems = (await this._manageSettingsService
                                        .GetSimpleMenuItemInfos(this.CurrentMenuItemsParentNodeId))
                                        .ToGridItemList();

        public async Task EnterMenuItemAsync(SimpleMenuItemInfo item)
        {
            if (".." != item.Name && this.MenuItems.Any(i => i.Data.ParentItemId.HasValue))
            {
                return;
            }

            this.CurrentMenuItemsParentNodeId = item.Name == ".." ?
                item.ParentItemId :
                item.Id;

            await this.LoadMenuItemsAsync();
        }

        public async Task MoveMenuItemAsync(SimpleMenuItemInfo menuItem, bool moveUp)
        {
            var orderedItems = this.MenuItems.OrderBy(i => i.Data.OrderNumber)
                                             .ToOrdinaryList();

            int secondItemIndex = orderedItems.IndexOf(menuItem);
            secondItemIndex += moveUp ? -1 : 1;
            
            if (secondItemIndex < 0)
            {
                secondItemIndex += orderedItems.Count;
            }
            else if (secondItemIndex >= orderedItems.Count)
            {
                secondItemIndex -= orderedItems.Count;
            }

            await this._manageSettingsService.SwapOrderNumbers(menuItem.Id, orderedItems[secondItemIndex].Id);
            await this.LoadMenuItemsAsync();

        }
        #endregion
    }
}
