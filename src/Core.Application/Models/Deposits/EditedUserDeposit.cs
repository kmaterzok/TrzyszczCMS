using Core.Application.Enums;
using Core.Shared.Models.ManageUser;

namespace Core.Application.Models.Deposits
{
    /// <summary>
    /// Message for depository and further processing during editing a user.
    /// </summary>
    public class EditedUserDeposit
    {
        /// <summary>
        /// Work mode of editor
        /// </summary>
        public DataEditorMode UserEditorMode { get; set; }

        /// <summary>
        /// Detailed info about user
        /// </summary>
        public DetailedUserInfo UserDetails { get; set; }
    }
}
