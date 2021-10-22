using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Core.Application.Helpers.Extensions
{
    /// <summary>
    /// The extensions methods for simpler <see cref="HttpResponseMessage"/>
    /// </summary>
    public static class ResponseExtensions
    {
        /// <summary>
        /// Get deserialised content from HTTP or default if the HTTP code was not success.
        /// </summary>
        /// <typeparam name="T">Deserialised data</typeparam>
        /// <param name="message">Response message from HTTP server</param>
        /// <returns>Task returning data</returns>
        public static async Task<T> ContentOrDefaultAsync<T>(this HttpResponseMessage message) where T : class
        {
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                return null;
            }
        }

    }
}
