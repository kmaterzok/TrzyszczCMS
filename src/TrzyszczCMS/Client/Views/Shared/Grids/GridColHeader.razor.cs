using Core.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace TrzyszczCMS.Client.Views.Shared.Grids
{
    /// <summary>
    /// The <see cref="Grid{TItem}"/>'s column header component.
    /// </summary>
    public partial class GridColHeader
    {
        #region Fields
        /// <summary>
        /// Value that starts the filtering range
        /// </summary>
        private DateTime? startDate;
        /// <summary>
        /// Value that ends the filtering range
        /// </summary>
        private DateTime? endDate;
        #endregion

        #region Parameters
        /// <summary>
        /// Displayed header title.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Display search box.
        /// </summary>
        [Parameter]
        public SearchBoxType SearchBoxType { get; set; } = SearchBoxType.TextInput;
        /// <summary>
        /// It defines width of the column in pixels.
        /// If the value is equal to 0 then the width is default.
        /// </summary>
        [Parameter]
        public int Width { get; set; }
        /// <summary>
        /// Invoked whenever search text is changed.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnSearchTextChanged { get; set; }
        /// <summary>
        /// Invoked whenever search dates are changed.
        /// </summary>
        [Parameter]
        public EventCallback<ValueRange<DateTime?>> OnSearchDateChanged { get; set; }
        #endregion

        #region Event handles
        private async Task OnStartDateChanging(ChangeEventArgs e)
        {
            if (this.OnSearchDateChanged.HasDelegate)
            {
                var range = new ValueRange<DateTime?>()
                {
                    Start = ParseToDate((string)e.Value),
                    End = this.endDate
                };
                await this.OnSearchDateChanged.InvokeAsync(range);
            }
        }

        private async Task OnEndDateChanging(ChangeEventArgs e)
        {
            if (this.OnSearchDateChanged.HasDelegate)
            {
                var range = new ValueRange<DateTime?>()
                {
                    Start = this.startDate,
                    End = ParseToDate((string)e.Value)
                };
                await this.OnSearchDateChanged.InvokeAsync(range);
            }
        }

        private DateTime? ParseToDate(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            return DateTime.ParseExact(s, "yyyy-MM-dd", null);
        }
            
        #endregion
    }
}
