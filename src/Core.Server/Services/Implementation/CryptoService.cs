using Core.Server.Helpers;
using Core.Server.Models;
using Core.Server.Models.Crypto;
using Core.Server.Models.Settings;
using Core.Server.Services.Interfaces;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;

namespace Core.Server.Services.Implementation
{
    /// <summary>
    /// The class implementing cryptographic handling of data for the backend.
    /// </summary>
    public class CryptoService : ICryptoService
    {
        #region Properties
        public CryptoSettings CryptoSettings { get; private set; }
        #endregion

        #region Ctor
        public CryptoService(IOptions<CryptoSettings> cryptoSettings)
        {
            CryptoSettings = cryptoSettings.Value;
            CryptoSettings.EnsureMinimumSecurity();
        }
        #endregion

        #region Interface methods
        public ArgonHashedPasswordData GenerateHashWithSalt(string password)
        {
            byte[] newSaltForPassword = CryptoHelper.GenerateRandomBytes(Constants.ARGON_PASSWORD_SALT_LENGTH);

            byte[] newArgonHash = this.GenerateHash(
                password,
                newSaltForPassword,
                Constants.ARGON_HASH_BYTES_QUANTITY,
                CryptoSettings.Argon2Password.Parallelism,
                CryptoSettings.Argon2Password.Iterations,
                CryptoSettings.Argon2Password.MemoryCost
            );

            return new ArgonHashedPasswordData()
            {
                HashedPassword = newArgonHash,
                PasswordDependentSalt = newSaltForPassword
            };
        }

        public bool PasswordValid(byte[] argonHash, byte[] passwordSalt, string password,
                                  int parallelism, int iterations, int memoryCost)
        {
            return this.GenerateHash(password, passwordSalt, argonHash.Length, parallelism, iterations, memoryCost)
                       .SequenceEqual(argonHash);
        }

        public AccessTokenVariants GenerateAccessToken()
        {
            byte[] plainAccessToken  = CryptoHelper.GenerateRandomBytes(Constants.ACCESS_TOKEN_BYTES_QUANTITY);
            byte[] hashedAccessToken = CryptoHelper.GenerateSha3_512(plainAccessToken, CryptoSettings.TokenHashIterations);

            return new AccessTokenVariants()
            {
                PlainToken = plainAccessToken,
                HashedToken = hashedAccessToken
            };
        }

        public byte[] GenerateHashFromPlainAccessToken(string plainAccessTokenFromBrowser)
        {
            var plain = Base64UrlEncoder.DecodeBytes(plainAccessTokenFromBrowser);
            return CryptoHelper.GenerateSha3_512(plain, CryptoSettings.TokenHashIterations);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generate Argon2 hash for desired password with desired salt.
        /// Hashing settings are loaded from <c>appsettings.json</c> and constants.
        /// </summary>
        /// <param name="password">Hashed password</param>
        /// <param name="passwordSalt">Salt for hash (dedicated one)</param>
        /// <param name="hashLength">Length of generated hash</param>
        /// <returns>Argon2 hash of password with salt</returns>
        private byte[] GenerateHash(string password, byte[] passwordSalt, int hashLength,
                                    int parallelism, int iterations, int memoryCost)
        {
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt                = passwordSalt;
                argon2.DegreeOfParallelism = parallelism;
                argon2.Iterations          = iterations;
                argon2.MemorySize          = memoryCost;

                return argon2.GetBytes(hashLength);
            }
        }
        #endregion
    }
}
