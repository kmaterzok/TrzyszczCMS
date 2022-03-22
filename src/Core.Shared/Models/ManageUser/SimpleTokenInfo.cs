using System;

namespace TrzyszczCMS.Core.Shared.Models.ManageUser
{
    /// <summary>
    /// The information about token that is assigned to a user.
    /// </summary>
    public class SimpleTokenInfo
    {
        /// <summary>
        /// Database row ID of the token.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The date of creating the token.
        /// </summary>
        public DateTime UtcCreateTime { get; set; }
        /// <summary>
        /// The date of creating the token.
        /// </summary>
        public DateTime UtcExpiryTime { get; set; }
        /// <summary>
        /// This token was used to sign in with an end user's web browser.
        /// </summary>
        public bool UsedForCurrentSession { get; set; }
    }
}
