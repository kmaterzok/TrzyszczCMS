namespace Core.Server.Models.Enums
{
    /// <summary>
    /// The reason of not uploading the file into the backend.
    /// </summary>
    public enum CreatingFileFailReason
    {
        FileSizeTooLarge,
        TooLargeMimeType
    }
}
