using Core.Shared.Models.PageContent;
using System.Collections.Generic;

namespace Core.Shared.Models.Rest.Responses.PageContent
{
    /// <summary>
    /// Content for the page to be displayed
    /// </summary>
    public class ModularPageContentResponse
    {
        /// <summary>
        /// The array of data for all of modules that are displayed on the page.
        /// </summary>
        public List<ModuleContent> ModuleContents { get; set; }
    }
}
