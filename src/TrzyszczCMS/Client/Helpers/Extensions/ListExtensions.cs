using System.Collections.Generic;

namespace TrzyszczCMS.Client.Helpers.Extensions
{
    /// <summary>
    /// Extension method for eased management of <see cref="List{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Swap <paramref name="item"/> with upper or lower item in <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">Data type used for the list</typeparam>
        /// <param name="list">Modified list</param>
        /// <param name="item">Item which the position is changed for</param>
        /// <param name="moveUp"><c>true</c> - swap with an upper item, <c>false</c> - swap with an lower item.</param>
        public static void MoveItem<T>(this List<T> list, T item, bool moveUp)
        {
            int itemIndex = list.IndexOf(item);
            int secondItemIndex = itemIndex;
            secondItemIndex += moveUp ? -1 : 1;

            if (secondItemIndex < 0)
            {
                secondItemIndex += list.Count;
            }
            else if (secondItemIndex >= list.Count)
            {
                secondItemIndex -= list.Count;
            }
            list.SwapItemsByIndex(itemIndex, secondItemIndex);
        }
        /// <summary>
        /// Swap items in <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">Data type used for the list</typeparam>
        /// <param name="list">Modified list</param>
        /// <param name="firstItemIndex">Index of the first swapped item</param>
        /// <param name="secondItemIndex">Index of the second swapped item</param>
        public static void SwapItemsByIndex<T>(this List<T> list, int firstItemIndex, int secondItemIndex)
        {
            var pivot = list[secondItemIndex];
            list[secondItemIndex] = list[firstItemIndex];
            list[firstItemIndex] = pivot;
        }
    }
}
