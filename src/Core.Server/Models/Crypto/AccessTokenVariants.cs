using System;

namespace Core.Server.Models.Crypto
{
    /// <summary>
    /// Data about token expressed in usable ways.
    /// </summary>
    public class AccessTokenVariants
    {
        /// <summary>
        /// Token as hash of plain token.
        /// </summary>
        public byte[] HashedToken { get; set; }
        /// <summary>
        /// Token data for user.
        /// </summary>
        public byte[] PlainToken { get; set; }

        public string GetPlainTokenForBrowserStorage()
        {
            return Convert.ToBase64String(PlainToken);
        }
    }
}
