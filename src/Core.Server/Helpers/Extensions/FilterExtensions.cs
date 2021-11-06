using Core.Shared.Enums;
using DAL.Models.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Server.Helpers.Extensions
{
    /// <summary>
    /// The class providing easing methods for simpler management and usage
    /// of <see cref="IQueryable"/> enumerables and filtering queried data.
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Filter data in <paramref name="source"/> using <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of <see cref="IQueryable"/>'s entry type (mostly generic one) in the input</typeparam>
        /// <typeparam name="TOut">Type of <see cref="IQueryable"/>'s entry type for output</typeparam>
        /// <param name="source">Filtered data</param>
        /// <param name="predicate">Filtering predicate</param>
        /// <returns>Filtered <see cref="IQueryable{Out}"/></returns>
        public static IQueryable<TOut> Where<TIn, TOut>(this IQueryable<TOut> source, Expression<Func<TIn, bool>> predicate) =>
            Queryable.Where(source as IQueryable<TIn>, predicate) as IQueryable<TOut>;

        /// <summary>
        /// Filter queried <paramref name="source"/> with <paramref name="filters"/>.
        /// </summary>
        /// <typeparam name="T">Type fo returned data entry after filtering</typeparam>
        /// <param name="source">Data before filtering</param>
        /// <param name="filters">Set of filters for apply</param>
        /// <returns>Data after filtering</returns>
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> source, Dictionary<FilteredGridField, string> filters)
        {
            foreach (var sgFilter in filters.Where(i => !string.IsNullOrEmpty(i.Value)))
            {
                source = source.ApplyFilter(sgFilter.Key, sgFilter.Value);
            }
            return source;
        }

        /// <summary>
        /// Apply a single filter onto <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of returned data entry after filtering</typeparam>
        /// <param name="gridField">A field which data will be filtered by</param>
        /// <param name="filterText">Text contained by a filtered parameter field</param>
        /// <returns>Filetred <see cref="IQueryable{T}"/> data</returns>
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, FilteredGridField gridField, string filterText)
        {
            switch (gridField)
            {
                case FilteredGridField.ManagePages_Articles_Title:
                case FilteredGridField.ManagePages_Posts_Title:
                    return source.Where<Cont_Page, T>(t => t.Name.Contains(filterText));

                case FilteredGridField.ManagePages_Articles_Created:
                case FilteredGridField.ManagePages_Posts_Created:
                    var range = FilterDataParser.ToDateRange(filterText);
                    if (range.Start.HasValue) { source = source.Where<Cont_Page, T>(t => t.CreateUtcTimestamp >= range.Start.Value); }
                    if (range.End.HasValue)   { source = source.Where<Cont_Page, T>(t => t.CreateUtcTimestamp <= range.End.Value);   }
                    return source;

                default:
                    throw new NotImplementedException("There are some enums that are not handled at all.");
                    // TODO: Write a test checking all the cases.
            }
        }
    }
}