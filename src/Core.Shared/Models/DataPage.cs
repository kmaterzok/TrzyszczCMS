using System.Collections.Generic;

namespace Core.Shared.Models
{
    /// <summary>
    /// Paginated portion of data  to process.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataPage<T>
    {
        /// <summary>
        /// Number of the current page.
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Are there any pages left to be fetched.
        /// </summary>
        public bool HasNextPage { get; set; }
        /// <summary>
        /// Are there any previous pages possible to be fetched.
        /// </summary>
        public bool HasPreviousPage { get; set; }
        /// <summary>
        /// The collection of entries in the page.
        /// </summary>
        public List<T> Entries { get; set; }
    }
}
