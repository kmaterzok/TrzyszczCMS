namespace Core.Application.Services.Interfaces
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
        void AddOrUpdate<T>(T objectToStore);
        /// <summary>
        /// Remove object from the depository if it has been already added.
        /// </summary>
        /// <typeparam name="T">Removed data type</typeparam>
        void Remove<T>();
        /// <summary>
        /// Get object from the depository.
        /// </summary>
        /// <typeparam name="T">Returned data type</typeparam>
        /// <param name="gotObject">Returned object</param>
        /// <returns>Object exists in the depository</returns>
        bool TryGet<T>(out T gotObject);
    }
}
