using Core.Application.Services.Interfaces;
using Core.Shared.Helpers.Extensions;
using System;
using System.Collections.Generic;

namespace Core.Application.Services.Implementation
{
    public class DataDepository : IDataDepository
    {
        #region Fields
        /// <summary>
        /// Store for all data in the escrow.
        /// </summary>
        private readonly Dictionary<Type, object> _deposits;
        #endregion

        #region Ctor
        public DataDepository()
        {
            this._deposits = new Dictionary<Type, object>();
        }
        #endregion

        #region Methods
        public void AddOrUpdate<T>(T objectToStore) =>
            this._deposits.AddOrUpdate(typeof(T), objectToStore);

        public void Remove<T>() =>
            this._deposits.Remove(typeof(T));

        public bool TryGet<T>(out T gotObject)
        {
            object returnedObject;
            bool found = this._deposits.TryGetValue(typeof(T), out returnedObject);
            gotObject = (T)returnedObject;
            return found;
        }
        #endregion
    }
}
