using System.IO;
using System.Threading;

namespace TrzyszczCMS.Core.Application.Models.Adapters
{
    /// <summary>
    /// The file info interface for an uploaded file in the client side.
    /// </summary>
    public interface IClientUploadedFile
    {
        /// <summary>
        /// Name of the file specified by a browser.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Uploaded file's MIME type.
        /// </summary>
        string ContentType { get; }
        /// <summary>
        /// Open a stream for reading content of the file
        /// </summary>
        /// <param name="maxAllowedSize">Maximum size of the read file.</param>
        /// <param name="cancellationToken">Token for cancelling the reading</param>
        /// <returns>Opened stream for reading</returns>
        Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default);
    }
}
