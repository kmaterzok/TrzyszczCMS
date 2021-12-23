using Core.Shared.Exceptions;

namespace Core.Server.Models.Settings
{
    /// <summary>
    ///  The settings for usage of cryptographic function Argon2 for storing passwords.
    /// </summary>
    public sealed class Argon2Settings
    {
        /// <summary>
        /// Quantity of iterations used for hasing
        /// </summary>
        public int Iterations { get; set; }
        /// <summary>
        /// Argon2 memory utilising setting expressed in kilobytes
        /// </summary>
        public int MemoryCost { get; set; }
        /// <summary>
        /// Argon2 parallelism setting
        /// </summary>
        public int Parallelism { get; set; }

        /// <summary>
        /// Check if the security settings complies with the minimum security values.
        /// </summary>
        public void EnsureMinimumSecurity()
        {
            if (this.Iterations < 4)
            {
                throw new InvalidMemberException($"Minimum value for {nameof(Iterations)} is 4.");
            }
            else if (this.MemoryCost < 64)
            {
                throw new InvalidMemberException($"Minimum value for {nameof(MemoryCost)} is 64 [kB].");
            }
            else if (this.Parallelism < 1)
            {
                throw new InvalidMemberException($"Minimum value for {nameof(Parallelism)} is 1.");
            }
        }
    }
}
