using TrzyszczCMS.Core.Application.Models.Adapters;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Threading;

namespace TrzyszczCMS.Client.Data.Model.Adapters
{
    public class ClientUploadedFileAdapter : IClientUploadedFile
    {
        #region Properties
        /// <summary>
        /// The object that is adaptered and indirectly accessed
        /// through the interface of the storing class.
        /// </summary>
        public IBrowserFile AdapteredObject { get; private set; }
        #endregion

        #region Ctor
        public ClientUploadedFileAdapter(IBrowserFile browserFile) =>
            this.AdapteredObject = browserFile;
        #endregion

        #region Interface implementation
        public string Name => this.AdapteredObject.Name;
        public string ContentType => this.AdapteredObject.ContentType;
        public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
            => this.AdapteredObject.OpenReadStream(maxAllowedSize, cancellationToken);
        #endregion
    }
}
