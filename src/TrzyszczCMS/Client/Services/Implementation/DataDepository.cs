using Blazored.SessionStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.Services.Interfaces;

namespace TrzyszczCMS.Client.Services.Implementations
{
    public class DataDepository : IDataDepository
    {
        #region Fields
        private readonly ISessionStorageService _sessionStorage;
        private readonly Lazy<Dictionary<Type, object>> _deposits;
        private readonly SemaphoredMonitor _lock;
        #endregion

        #region Ctor
        public DataDepository(ISessionStorageService sessionStorage)
        {
            this._sessionStorage = sessionStorage;
            this._deposits = new Lazy<Dictionary<Type, object>>();
            this._lock = new SemaphoredMonitor();
        }
        #endregion

        #region Methods
        public async Task AddOrUpdateAsync<T>(T objectToStore, bool surviveRefresh = true) where T : class
        {
            await this._lock.InvokeAsync(async () =>
            {
                await this.UnguardedRemoveAsync<T>();
                if (surviveRefresh)
                {
                    await this._sessionStorage.SetItemAsync(typeof(T).FullName, objectToStore);
                }
                else
                {
                    this._deposits.Value.Add(typeof(T), objectToStore);
                }
            });
        }

        public async Task RemoveAsync<T>() where T : class =>
            await this._lock.InvokeAsync(async () => await this.UnguardedRemoveAsync<T>());

        public async Task<T> GetAsync<T>() where T : class
        {
            return await this._lock.InvokeAsync(async () =>
            {
                object returnedObject;
                if (await _sessionStorage.ContainKeyAsync(typeof(T).FullName))
                {
                    return await _sessionStorage.GetItemAsync<T>(typeof(T).FullName);
                }
                else if (this._deposits.Value.TryGetValue(typeof(T), out returnedObject))
                {
                    return (T)returnedObject;
                }
                return null;
            });
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Remove an item without concurrent-safe execution.
        /// </summary>
        /// <typeparam name="T">Type of the removed item</typeparam>
        /// <returns>Task of this method</returns>
        private async Task UnguardedRemoveAsync<T>() where T : class
        {
            if (await _sessionStorage.ContainKeyAsync(typeof(T).FullName))
            {
                await _sessionStorage.RemoveItemAsync(typeof(T).FullName);
            }
            else
            {
                this._deposits.Value.Remove(typeof(T));
            }
        }
        #endregion
    }
}
