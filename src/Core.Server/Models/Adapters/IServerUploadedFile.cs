using System.IO;

namespace Core.Server.Models.Adapters
{
    /// <summary>
    /// The file info interface for an uploaded file in the server side.
    /// </summary>
    public interface IServerUploadedFile
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
        /// The file name from the Content-Disposition header
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// Copy uploaded file to a specified location.
        /// </summary>
        /// <param name="target">Target stream for storing file's data</param>
        void CopyTo(Stream target);
    }
}
