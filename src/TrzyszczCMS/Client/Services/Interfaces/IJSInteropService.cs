using System;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model.JSInterop;

namespace TrzyszczCMS.Client.Services.Interfaces
{
    /// <summary>
    /// Interface providing invoking functions of JavaScript.
    /// </summary>
    public interface IJSInteropService
    {
        /// <summary>
        /// Get range of text selection.
        /// </summary>
        /// <param name="componentId">Id of the HTML component which will be processed by JavaScript</param>
        /// <returns>Task returning text range indexes</returns>
        Task<SelectionRange> GetSelectionRangeAsync(string componentId);
        /// <summary>
        /// Set index of text cursor.
        /// </summary>
        /// <param name="componentId">Id of the HTML component which will be processed by JavaScript</param>
        /// <param name="index">Index of cursor in the text</param>
        /// <param name="stateHasChanged">Invoked for indicating change in the component, that invokes this method.</param>
        /// <returns>Task that executes JS</returns>
        Task SelectTextIndexAsync(string componentId, int index, Action stateHasChanged);
        /// <summary>
        /// Set range of text selection.
        /// </summary>
        /// <param name="componentId">Id of the HTML component which will be processed by JavaScript</param>
        /// <param name="start">Starting index of selection</param>
        /// <param name="end">Ending index of selection</param>
        /// <param name="stateHasChanged">Invoked for indicating change in the component, that invokes this method.</param>
        /// <returns>Task that executes JS</returns>
        Task SelectTextRangeAsync(string componentId, int start, int end, Action stateHasChanged);
    }
}
