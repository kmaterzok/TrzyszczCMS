using Core.Application.Enums;
using Core.Shared.Models.ManagePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Models.Deposits
{
    /// <summary>
    /// Message for depository and further processing during editing pages.
    /// </summary>
    public class EditedPageDeposit
    {
        /// <summary>
        /// Work mode of editor
        /// </summary>
        public PageEditorMode PageEditorMode { get; set; }
        /// <summary>
        /// All necessary info for managing and editing
        /// </summary>
        public DetailedPageInfo PageDetails { get; set; }
    }
}
