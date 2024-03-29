﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Helpers
{
    /// <summary>
    /// A simple lock / mutex for invoking tasks within async methods.
    /// </summary>
    public class SemaphoredLock : IDisposable
    {
        #region Fields
        /// <summary>
        /// The semaphore guarding concurrent execution of methods.
        /// </summary>
        private SemaphoreSlim _semaphore;
        #endregion

        #region Ctor
        public SemaphoredLock()
        {
            this._semaphore = new SemaphoreSlim(1, 1);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Invoke method and execute it synchronously depending on lock value.
        /// </summary>
        /// <typeparam name="T">Type of the returned value</typeparam>
        /// <param name="task">Executed / invoked task</param>
        /// <param name="configureAwait">It says if the executed <paramref name="task"/> must execute within the same context as the semaphore.</param>
        /// <returns>Task returning specified value</returns>
        public async Task<T> InvokeAsync<T>(Func<Task<T>> task, bool configureAwait = true)
        {
            await this._semaphore.WaitAsync().ConfigureAwait(configureAwait);
            try
            {
                return await task.Invoke();
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
        /// <param name="task">Executed / invoked task</param>
        /// <param name="configureAwait">It says if the executed <paramref name="task"/> must execute within the same context as the semaphore.</param>
        /// <returns>Task returning specified value</returns>
        public async Task InvokeAsync(Func<Task> task, bool configureAwait = true)
        {
            await this._semaphore.WaitAsync().ConfigureAwait(configureAwait);
            try
            {
                await task.Invoke();
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
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (null != this._semaphore)
            {
                this._semaphore.Dispose();
                this._semaphore = null;
            }
        }
        ~SemaphoredLock() => Dispose();
        #endregion
    }
}
