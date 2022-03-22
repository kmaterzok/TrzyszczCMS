using TrzyszczCMS.Core.Server.Models.Crypto;
using TrzyszczCMS.Core.Server.Models.Settings;

namespace TrzyszczCMS.Core.Server.Services.Interfaces
{
    /// <summary>
    /// The interface for instances providing cryptographic functions and handling in the backend.
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Cryptographic settings used in the backend for securing passwords and tokens.
        /// </summary>
        CryptoSettings CryptoSettings { get; }

        /// <summary>
        /// Check if <paramref name="argonHash"/> with specified <paramref name="passwordSalt"/> matches the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="argonHash">Argon2 hash of password with salt</param>
        /// <param name="passwordSalt">Salt used during hashing the password</param>
        /// <param name="password">Plain password for check</param>
        /// <param name="parallelism">Parallelism of hashing</param>
        /// <param name="iterations">Iterations of hashing</param>
        /// <param name="memoryCost">Occupied memory [kB] for hashing</param>
        /// <returns></returns>
        bool PasswordValid(byte[] argonHash, byte[] passwordSalt, string password,
                                  int parallelism, int iterations, int memoryCost);
        /// <summary>
        /// Generate hash for password with cryptographically random salt used for the hash.
        /// </summary>
        /// <param name="password">Password for which the hash will be created</param>
        /// <returns>Data that must be stored for future password validation</returns>
        ArgonHashedPasswordData GenerateHashWithSalt(string password);

        /// <summary>
        /// Generate access token with its complementary hash for storing.
        /// </summary>
        /// <returns>Access token with its hash</returns>
        AccessTokenVariants GenerateAccessToken();

        /// <summary>
        /// Generate hash from plain access token from browser.
        /// </summary>
        /// <param name="plainAccessTokenFromBrowser">Access token in a format for storing in a browser</param>
        /// <returns>Generates hash of token</returns>
        byte[] GenerateHashFromPlainAccessToken(string plainAccessTokenFromBrowser);
    }
}
