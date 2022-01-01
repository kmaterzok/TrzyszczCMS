using Core.Server.Models.Adapters;
using Core.Server.Models.Enums;
using Core.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Core.Server.Helpers
{
    /// <summary>
    /// The helper for easing handling of file data.
    /// </summary>
    public static class FileManagementHelper
    {
        /// <summary>
        /// Check if any file does not meet compliance criteria before upload.
        /// </summary>
        /// <param name="files">Checked files' info</param>
        /// <returns>Reason of not creating a file</returns>
        public static CreatingFileFailReason? CheckFilesBeforeUpload(IEnumerable<IServerUploadedFile> files)
        {
            if (files.Any(i => i.Length > CommonConstants.MAX_UPLOADED_FILE_LENGTH_BYTES))
            {
                return CreatingFileFailReason.FileSizeTooLarge;
            }
            else if (files.Any(i => i.ContentType.Length > 100))
            {
                return CreatingFileFailReason.TooLargeMimeType;
            }
            return null;
        }
    }
}
