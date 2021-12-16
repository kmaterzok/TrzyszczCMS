using Core.Shared.Enums;
using System.Collections.Generic;

namespace Core.Shared.Models.Rest.Requests.ManageFiles
{
    public class SimpleFileInfoRequest
    {
        /// <summary>
        /// Row ID of the file node that stores all requested files.
        /// </summary>
        public int? FileNodeId{ get; set; }
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
