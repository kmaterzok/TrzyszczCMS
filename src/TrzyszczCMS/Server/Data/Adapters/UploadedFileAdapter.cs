using Core.Server.Models.Adapters;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TrzyszczCMS.Server.Data.Adapters
{
    /// <summary>
    /// The adaptering class for handling file info objects.
    /// The class used for file upload.
    /// </summary>
    public class UploadedFileAdapter : IUploadedFile
    {
        #region Properties
        /// <summary>
        /// The object that is adaptered and indirectly accessed
        /// through the interface of the storing class.
        /// </summary>
        public IFormFile AdapteredObject { get; private set; }
        #endregion

        #region Ctor
        public UploadedFileAdapter(IFormFile formFile) =>
            this.AdapteredObject = formFile;
        #endregion

        #region Methods
        public string ContentType => this.AdapteredObject.ContentType;
        public long Length => this.AdapteredObject.Length;
        public string Name => this.AdapteredObject.Name;
        public string FileName => this.AdapteredObject.FileName;
        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) =>
            await this.AdapteredObject.CopyToAsync(target, cancellationToken);
        #endregion
    }
}
