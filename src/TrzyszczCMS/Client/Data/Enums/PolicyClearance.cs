namespace TrzyszczCMS.Client.Data.Enums
{
    /// <summary>
    /// Clearances for accessing specific places and functionalities by an end user.
    /// Clearances are based on policies.
    /// </summary>
    public enum PolicyClearance
    {
        #region --- Navigation bar ---
        /// <summary>
        /// Possibility of accessing navigation bar settings.
        /// </summary>
        AccessNavBarSettings,
        #endregion

        #region --- Users ---
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
        #endregion

        #region --- Files ---
        /// <summary>
        /// Uploading files and creating directories.
        /// </summary>
        AllowFilesAdding,
        /// <summary>
        /// Deleting files and directories.
        /// </summary>
        AllowFilesDeleting,
        #endregion

        #region --- Pages ---
        /// <summary>
        /// Displaying managed posts list. Only if there are any policies for managing posts.
        /// </summary>
        DisplayPostsForManaging,
        /// <summary>
        /// Displaying managed articles list. Only if there are any policies for managing articles.
        /// </summary>
        DisplayArticlesForManaging,
        /// <summary>
        /// Displaying a link / view for homepage management.
        /// </summary>
        DisplayHomepageForManaging,
        /// <summary>
        /// Deleting posts from the database.
        /// </summary>
        AllowPostsDeleting,
        /// <summary>
        /// Deleting articles from the database.
        /// </summary>
        AllowArticlesDeleting,
        /// <summary>
        /// Adding posts into the database.
        /// </summary>
        AllowPostsAdding,
        /// <summary>
        /// Adding articles into the database.
        /// </summary>
        AllowArticlesAdding,
        /// <summary>
        /// Editing posts from the database.
        /// </summary>
        AllowPostsEditing,
        /// <summary>
        /// Editing articles from the database.
        /// </summary>
        AllowArticlesEditing,
        /// <summary>
        /// Editing the homepage.
        /// </summary>
        AllowHomepageEditing,
        #endregion
    }
}
