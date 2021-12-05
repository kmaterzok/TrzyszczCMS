using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Core.Server.Helpers
{
    /// <summary>
    /// The class containing cryptographic helper methods
    /// </summary>
    public static class CryptoHelper
    {
        /// <summary>
        /// Generate a cryptographically safe sequence of random bytes.
        /// </summary>
        /// <param name="quantityOfBytes">Quantity of bytes in the returned bytes sequence</param>
        /// <returns>Random bytes sequence</returns>
        public static byte[] GenerateRandomBytes(int quantityOfBytes)
        {
            var buffer = new byte[quantityOfBytes];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
                return buffer;
            }
        }
        /// <summary>
        /// Generate a SHA3-512 hash of <paramref name="data"/>
        /// </summary>
        /// <param name="data">Data for hashing</param>
        /// <param name="iterations">Quantity of hashing iterations for resistance against brute force</param>
        /// <returns>Hashed of data</returns>
        public static byte[] GenerateSha3_512(byte[] data, uint iterations = 1)
        {
            var keccak = new Org.BouncyCastle.Crypto.Digests.Sha3Digest(512);
            byte[] finalHash = new byte[64]; // 512 / 8 = 64
            
            for (uint i=0; i < iterations; ++i)
            {
                keccak.BlockUpdate(data, 0, data.Length);
                keccak.DoFinal(finalHash, 0);
                data = finalHash;
            }
            return finalHash;
        }
        /// <summary>
        /// Generate a random cryptographically safe ASCII-compliant string.
        /// </summary>
        /// <param name="quantityOfCharacters">Quantity of characters in the returned string</param>
        /// <returns>Random string</returns>
        public static string GenerateRandomString(int quantityOfCharacters)
        {
            var randomBytes = GenerateRandomBytes(quantityOfCharacters)
                .Select(i => (byte)((i % 94) + 33))
                .ToArray();
            return Encoding.ASCII.GetString(randomBytes);
        }
    }
}
