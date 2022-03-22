using Microsoft.IdentityModel.Tokens;

namespace TrzyszczCMS.Core.Server.Models.Crypto
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

        /// <summary>
        /// Get token formatted as URL safe Base64 string.
        /// </summary>
        /// <returns>Base64Url token</returns>
        public string GetPlainTokenForBrowserStorage() =>
            Base64UrlEncoder.Encode(PlainToken);
    }
}
