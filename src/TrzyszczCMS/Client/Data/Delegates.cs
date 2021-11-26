namespace TrzyszczCMS.Client.Data
{
    /// <summary>
    /// Delegate of change in the content.
    /// </summary>
    /// <typeparam name="T">Type of data that was changed</typeparam>
    /// <param name="newContent">New content that is present</param>
    public delegate void ChangeContentHandler<T>(T newContent);
    /// <summary>
    /// Delegate of checking the validity of the checked object's value.
    /// </summary>
    /// <param name="checkedObject">The object to be checked</param>
    /// <returns>Is the checked object valid</returns>
    public delegate bool ValidityCheckHandler(object checkedObject);
}
