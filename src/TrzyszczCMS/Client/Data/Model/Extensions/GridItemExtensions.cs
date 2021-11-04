using System.Collections.Generic;
using System.Linq;

namespace TrzyszczCMS.Client.Data.Model.Extensions
{
    /// <summary>
    /// The class of helpers used for transforming data for <see cref="Views.Shared.Grids.Grid{TItem}"/>.
    /// </summary>
    public static class GridItemExtensions
    {
        /// <summary>
        /// Remap enumerable between <see cref="List{T}<typeparamref name="T"/>"/> and grid-ready list.
        /// </summary>
        /// <typeparam name="T">Type of data for a single row</typeparam>
        /// <param name="source">Source enumerable</param>
        /// <returns>Grid-ready list</returns>
        public static List<GridItem<T>> ToGridItemList<T>(this IEnumerable<T> source)
        {
            return source.Select(i => new GridItem<T>()
            {
                Checked = false,
                Data = i
            }).ToList();
        }
    }
}
