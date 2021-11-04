using Core.Shared.Enums;
using System.Collections.Generic;

namespace Core.Shared.Models.Rest.Requests.ManagePages
{
    public class SimplePageInfoRequest
    {
        /// <summary>
        /// Type of desired pages
        /// </summary>
        public PageType Type { get; set; }
        /// <summary>
        /// Fetched page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Filters applied for fetching data
        /// </summary>
        public Dictionary<FilteredGridField, string> Filters { get; set; }
    }
}
