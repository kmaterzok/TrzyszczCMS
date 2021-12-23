namespace Core.Server.Models.Settings
{
    /// <summary>
    /// The configuration for file storage.
    /// </summary>
    public sealed class StorageSettings
    {
        /// <summary>
        /// The path of the directory that stores all files in the storage.
        /// </summary>
        public string Path { get; set; }
    }
}
