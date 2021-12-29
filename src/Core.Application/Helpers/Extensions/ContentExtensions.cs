using Core.Application.Models;
using Core.Application.Models.Adapters;
using Core.Shared.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.Application.Helpers.Extensions
{
    /// <summary>
    /// The class with methods that ease handling of content-oriented information,
    /// e.g. for sending form data like files.
    /// </summary>
    public static class ContentExtensions
    {
        /// <summary>
        /// Create a <see cref="ByteArrayContent"/> instance ready for upload.
        /// </summary>
        /// <param name="file">Uploaded file information</param>
        /// <returns>Task returning an instance of content prepared for upload</returns>
        public static async Task<ByteArrayContent> CreateByteArrayContentAsync(IClientUploadedFile file)
        {
            using (var fileStream = file.OpenReadStream(CommonConstants.MAX_UPLOADED_FILE_LENGTH_BYTES))
            {
                ByteArrayContent fileContent = new (await new StreamContent(fileStream).ReadAsByteArrayAsync());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(Constants.FILE_UPLOAD_DISPOSITION_TYPE)
                {
                    FileName = file.Name
                };
                return fileContent;
            }
        }
    }
}
