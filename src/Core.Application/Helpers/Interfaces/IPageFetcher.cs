using TrzyszczCMS.Core.Shared.Models;
using System.Threading.Tasks;

namespace TrzyszczCMS.Core.Application.Helpers.Interfaces
{
    /// <summary>
    /// Interface of abstraction for getting paginated data.
    /// </summary>
    public interface IPageFetcher<T>
    {
        /// <summary>
        /// Are there any pages left to be fetched.
        /// </summary>
        bool HasNext { get; }
        /// <summary>
        /// Are there any previous pages possible to be fetched.
        /// </summary>
        bool HasPrevious { get; }
        /// <summary>
        /// Currently pointed page's number.
        /// </summary>
        int CurrentPageNumber { get; }
        /// <summary>
        /// Get page pointed by current number.
        /// </summary>
        /// <returns>Task returning page of data</returns>
        Task<DataPage<T>> GetCurrent();
        /// <summary>
        /// Get next page from the source.
        /// </summary>
        /// <returns>Task returning page of data</returns>
        Task<DataPage<T>> GetNext();
        /// <summary>
        /// Get next page from the source.
        /// </summary>
        /// <returns>Task returning page of data</returns>
        Task<DataPage<T>> GetPrevious();
    }
}
