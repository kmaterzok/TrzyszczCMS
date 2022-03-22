using System;
using System.Threading;

namespace TrzyszczCMS.Core.Server.Helpers
{
    /// <summary>
    /// The class that lets accessing a value in a synchronous way.
    /// </summary>
    /// <typeparam name="T">Type of guarded value</typeparam>
    public class SemaphoredValue<T>
    {
        #region Fields
        /// <summary>
        /// The semaphore guarding concurrent execution of methods.
        /// </summary>
        private SemaphoreSlim _semaphore;
        /// <summary>
        /// THe synchronised value.
        /// </summary>
        private T _value;
        #endregion

        #region Ctor
        public SemaphoredValue(Func<T> allocator)
        {
            this._value = allocator.Invoke();
            this._semaphore = new SemaphoreSlim(1, 1);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Invoke method and execute it synchronously depending on lock value.
        /// </summary>
        /// <param name="action">Executed / invoked action</param>
        public void Synchronise(Action<SemaphoredValue<T>> action)
        {
            this._semaphore.Wait();
            try
            {
                action.Invoke(this);
            }
            catch
            {
                throw;
            }
            finally
            {
                this._semaphore.Release();
            }
        }
        /// <summary>
        /// Invoke method and execute it synchronously depending on lock value.
        /// </summary>
        /// <typeparam name="TOut">Type of returned object</typeparam>
        /// <param name="action">Executed / invoked action</param>
        /// <returns>Returned data from sycnhronously executed action</returns>
        public TOut Synchronise<TOut>(Func<SemaphoredValue<T>, TOut> action)
        {
            this._semaphore.Wait();
            try
            {
                return action.Invoke(this);
            }
            catch
            {
                throw;
            }
            finally
            {
                this._semaphore.Release();
            }
        }
        /// <summary>
        /// Invoke method and execute it synchronously depending on lock value.
        /// </summary>
        /// <param name="action">Executed / invoked action</param>
        public void Invoke(Action<T> action)
        {
            if (_semaphore.CurrentCount == 1)
            {
                throw new InvalidOperationException($"Invoke this method as action for {nameof(Synchronise)} method.");
            }
            action.Invoke(this._value);
        }
        /// <summary>
        /// Invoke method and execute it synchronously depending on lock value.
        /// </summary>
        /// <param name="action">Executed / invoked action</param>
        /// <returns>A value from <paramref name="action"/></returns>
        public TOut Invoke<TOut>(Func<T,TOut> action)
        {
            if (_semaphore.CurrentCount == 1)
            {
                throw new InvalidOperationException($"Invoke this method as action for {nameof(Synchronise)} method.");
            }
            return action.Invoke(this._value);
        }
        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="setValue">A value that is set</param>
        public void SetValue(T setValue)
        {
            if (_semaphore.CurrentCount == 1)
            {
                throw new InvalidOperationException($"Invoke this method as action for {nameof(Synchronise)} method.");
            }
            this._value = setValue;
        }
        #endregion
    }
}
