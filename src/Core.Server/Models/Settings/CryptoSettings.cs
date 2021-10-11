using Core.Server.Models.Exceptions;

namespace Core.Server.Models.Settings
{
    /// <summary>
    /// The class containing settings of cryptography for backend usage.
    /// </summary>
    public class CryptoSettings
    {
        /// <summary>
        /// How many iterations of SHA3-512 (Keccak) must be done to achieve a hashed access token.
        /// </summary>
        public uint TokenHashIterations { get; set; }
        /// <summary>
        /// Settings for password hashing with Argon2 function.
        /// </summary>
        public Argon2Settings Argon2Password { get; set; }

        /// <summary>
        /// Check if the security settings complies with minimum security values.
        /// </summary>
        public void EnsureMinimumSecurity()
        {
            if (TokenHashIterations < 5000)
            {
                throw new InvalidMemberException($"The {nameof(TokenHashIterations)} below 5000 is not secure enough.");
            }
            this.Argon2Password.EnsureMinimumSecurity();
        }
    }
}
