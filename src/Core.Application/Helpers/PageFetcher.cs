using Core.Application.Helpers.Interfaces;
using Core.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Core.Application.Helpers
{
    /// <summary>
    /// Delegate for getting page of data for a given page number.
    /// </summary>
    /// <typeparam name="T">Type of data present within a single item of collection.</typeparam>
    /// <param name="fetchedPageNumber">Fetched page's number</param>
    /// <returns>A page of data</returns>
    public delegate Task<DataPage<T>> FetchDataPageAsyncHandler<T>(int fetchedPageNumber);

    /// <summary>
    /// Abstraction for getting paginated data.
    /// </summary>
    public class PageFetcher<T> : IPageFetcher<T>
    {
        #region Fields
        /// <summary>
        /// Fetches data from the data source.
        /// </summary>
        private readonly FetchDataPageAsyncHandler<T> _fetchPage;
        /// <summary>
        /// Indicates if the method <see cref="GetCurrent"/> was already invoked.
        /// </summary>
        private bool _alreadyGotCurrentPage;
        #endregion

        #region Properties
        public int CurrentPageNumber { get; private set; }
        public bool HasNext { get; private set; }
        public bool HasPrevious { get; private set; }
        #endregion

        #region Ctor
        public PageFetcher(int desiredPageNumber, FetchDataPageAsyncHandler<T> fetchPageHandler)
        {
            this.HasNext = false;
            this.HasPrevious = false;
            this.CurrentPageNumber = desiredPageNumber;
            this._fetchPage = fetchPageHandler;
            this._alreadyGotCurrentPage = false;
        }
        #endregion

        #region Methods
        public async Task<DataPage<T>> GetCurrent()
        {
            var pageOfData = await this._fetchPage(this.CurrentPageNumber);
            
            this.HasPrevious = pageOfData.HasPreviousPage;
            this.HasNext     = pageOfData.HasNextPage;
            this._alreadyGotCurrentPage = true;

            return pageOfData;
        }
        public async Task<DataPage<T>> GetNext()
        {
            this.EnsureIfAlreadyGotCurrent();
            return this.HasNext ? await this.GetDataAndSetProperties(true) : null;
        }
        
        public async Task<DataPage<T>> GetPrevious()
        {
            this.EnsureIfAlreadyGotCurrent();
            return this.HasPrevious ? await this.GetDataAndSetProperties(false) : null;
        }

        private async Task<DataPage<T>> GetDataAndSetProperties(bool gettingNextPage)
        {
            this.CurrentPageNumber += gettingNextPage ? 1 : -1;
            
            var pageOfData = await this._fetchPage.Invoke(this.CurrentPageNumber);

            if (null == pageOfData)
            {
                if (gettingNextPage) { HasNext = false; }
                else                 { HasPrevious = false; }
            }
            else
            {
                this.HasPrevious = pageOfData.HasPreviousPage;
                this.HasNext     = pageOfData.HasNextPage;
            }
            return pageOfData;
        }

        private void EnsureIfAlreadyGotCurrent()
        {
            if (!this._alreadyGotCurrentPage)
            {
                throw new InvalidOperationException($"Method {nameof(GetCurrent)} must be invoked.");
            }
        }
        #endregion
    }
}
