namespace TrzyszczCMS.Client.Data.Enums
{
    /// <summary>
    /// Clearances for accessing specific places and functionalities by an end user.
    /// Clearances are based on policies.
    /// </summary>
    public enum PolicyClearance
    {
        /// <summary>
        /// Possibility of accessing navigation bar settings.
        /// </summary>
        AccessNavBarSettings,
        /// <summary>
        /// Displaying managed users list. Only if there are any policies for managing users.
        /// </summary>
        DisplayUsersForManaging,
        /// <summary>
        /// Deleting users from the database.
        /// </summary>
        AllowUsersDeleting,
        /// <summary>
        /// Editing users and saving changes in the database.
        /// </summary>
        AllowUsersEditing,
        /// <summary>
        /// Adding users into the database.
        /// </summary>
        AllowUsersAdding,
        /// <summary>
        /// Uploading files and creating directories.
        /// </summary>
        AllowFilesAdding,
        /// <summary>
        /// Deleting files and directories.
        /// </summary>
        AllowFilesDeleting,
    }
}
