namespace Core.Shared.Models.ManageSettings
{
    /// <summary>
    /// The info about a single item
    /// that is presented in a menu.
    /// </summary>
    public class SimpleMenuItemInfo
    {
        /// <summary>
        /// Row ID of the item.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Row ID of the item that holds this item.
        /// </summary>
        public int? ParentItemId { get; set; }
        /// <summary>
        /// Displayed name of the item
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// URI or link to the resource or page that
        /// will be presented after interaction with this item.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// It lets set order of items in the menu.
        /// </summary>
        public int OrderNumber { get; set; }
    }
}
