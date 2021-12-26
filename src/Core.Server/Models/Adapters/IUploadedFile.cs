using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Server.Models.Adapters
{
    /// <summary>
    /// The file info interface for an uploaded file.
    /// </summary>
    public interface IUploadedFile
    {
        /// <summary>
        /// The file's content type
        /// </summary>
        string ContentType { get; }
        /// <summary>
        /// Byte length of the file
        /// </summary>
        long Length { get; }
        /// <summary>
        /// The form field name from the Content-Disposition header
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The file name from the Content-Disposition header
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// Copy uploaded file to a specified location.
        /// </summary>
        /// <param name="target">Target stream for storing file's data</param>
        /// <param name="cancellationToken">Token for cancelling the executed task</param>
        /// <returns>Task executing copying</returns>
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}
