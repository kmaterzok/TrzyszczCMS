using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Services.Interfaces
{
    /// <summary>
    /// Interface for a data storage that stores
    /// any object for future purposes.
    /// Objects distinguished by their own types.
    /// </summary>
    public interface IDataDepository
    {
        /// <summary>
        /// Store <paramref name="objectToStore"/> in the depository.
        /// Update stored object if it has been already added.
        /// </summary>
        /// <typeparam name="T">Stored data type</typeparam>
        /// <param name="objectToStore">Object to store</param>
        /// <param name="surviveRefresh">Added object survives page refresh and still exists</param>
        /// <returns>Task adding the data</returns>
        Task AddOrUpdateAsync<T>(T objectToStore, bool surviveRefresh = true) where T : class;
        /// <summary>
        /// Remove object from the depository if it has been already added.
        /// </summary>
        /// <typeparam name="T">Removed data type</typeparam>
        /// <returns>Task removing the data</returns>
        Task RemoveAsync<T>() where T : class;
        /// <summary>
        /// Get object from the depository.
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <returns>Task returning the found object</returns>
        Task<T> GetAsync<T>() where T : class;
    }
}
