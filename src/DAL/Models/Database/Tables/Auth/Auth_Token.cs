using System;

namespace DAL.Models.Database.Tables
{
    /// <summary>
    /// Information about access (authentication) tokens.
    /// </summary>
    public class Auth_Token
    {
        /// <summary>
        /// Row ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Access token used for authorisation
        /// </summary>
        public byte[] HashedToken { get; set; }
        /// <summary>
        /// Row ID of the authenticated user.
        /// </summary>
        public int Auth_UserId { get; set; }
        /// <summary>
        /// UTC time of the token expiriation.
        /// </summary>
        public DateTime UtcExpiryTime { get; set; }
    }
}
