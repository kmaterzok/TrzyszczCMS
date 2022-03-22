using TrzyszczCMS.Core.Server.Models.Adapters;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TrzyszczCMS.Server.Data.Adapters
{
    /// <summary>
    /// The adaptering class for handling file info objects.
    /// The class used for file upload.
    /// </summary>
    public class ServerUploadedFileAdapter : IServerUploadedFile
    {
        #region Properties
        /// <summary>
        /// The object that is adaptered and indirectly accessed
        /// through the interface of the storing class.
        /// </summary>
        public IFormFile AdapteredObject { get; private set; }
        #endregion

        #region Ctor
        public ServerUploadedFileAdapter(IFormFile formFile) =>
            this.AdapteredObject = formFile;
        #endregion

        #region Interface implementation
        public string ContentType => this.AdapteredObject.ContentType;
        public long Length => this.AdapteredObject.Length;
        public string FileName => this.AdapteredObject.FileName;
        public void CopyTo(Stream target) => this.AdapteredObject.CopyTo(target);
        #endregion
    }
}
