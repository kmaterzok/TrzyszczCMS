using System;

namespace TrzyszczCMS.Core.Shared.Models.ManageFiles
{
    /// <summary>
    /// The basic inforomation about an uploaded file or a logical directory.
    /// </summary>
    public class SimpleFileInfo
    {
        /// <summary>
        /// Row ID of the file info entry in te he database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Row ID of the file info that is a directory and holds files.
        /// These files are logically stored in this folder.
        /// </summary>
        public int? ParentFileId { get; set; }
        /// <summary>
        /// The entry is a logical direcotry for storing files.
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// Timestamp of the creating the element in the database and in a store.
        /// </summary>
        public DateTime CreationUtcTimestamp { get; set; }
        /// <summary>
        /// Name of the stored file or a logical directory depengin on the <see cref="IsDirectory"/> flag.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The GUID allowing the client to access the file through Web API.
        /// It is also the real name of the file stored in the storage.
        /// </summary>
        public Guid AccessGuid { get; set; }
        /// <summary>
        /// The file's MIME type.
        /// </summary>
        public string MimeType { get; set; }
    }
}
