namespace TrzyszczCMS.Client.Data
{
    /// <summary>
    /// Delegate of change in the content.
    /// </summary>
    /// <typeparam name="T">Type of data that was changed</typeparam>
    /// <param name="newContent">New content that is present</param>
    public delegate void ChangeContentHandler<T>(T newContent);
}
