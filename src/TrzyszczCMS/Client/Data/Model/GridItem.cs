namespace TrzyszczCMS.Client.Data.Model
{
    /// <summary>
    /// The item holding data for a grid row
    /// </summary>
    /// <typeparam name="T">Type of data stored in the item</typeparam>
    public class GridItem<T>
    {
        /// <summary>
        /// Id item checked in the table
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// Displayed item data
        /// </summary>
        public T Data { get; set; }
    }
}
