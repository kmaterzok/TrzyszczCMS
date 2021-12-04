using Core.Application.Enums;
using Core.Shared.Models.ManagePage;

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
        public DataEditorMode PageEditorMode { get; set; }
        /// <summary>
        /// Currently used and displayed management tool for the edited / created page.
        /// </summary>
        public PageManagementTool CurrentManagementTool { get; set; }
        /// <summary>
        /// All necessary info for managing and editing
        /// </summary>
        public DetailedPageInfo PageDetails { get; set; }
        /// <summary>
        /// The index of the module that is being edited.
        /// </summary>
        public int EditedModuleListIndex { get; set; }
        /// <summary>
        /// THe original value of the <see cref="DetailedPageInfo.UriName"/>, before modification.
        /// </summary>
        public string OldUriName { get; set; }
    }
}
