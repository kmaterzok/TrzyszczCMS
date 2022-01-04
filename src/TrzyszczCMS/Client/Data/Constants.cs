using Core.Shared.Enums;

namespace TrzyszczCMS.Client.Data
{
    /// <summary>
    /// The class for storing frequently used values in constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Name of the variable storing access token placed in the Local Storage.
        /// </summary>
        public const string LOCAL_STORAGE_ACCESS_TOKEN_VAR_NAME = "accessToken";
        /// <summary>
        /// Name of the authentication type used in the application.
        /// </summary>
        public const string APPLICATION_AUTH_TYPE_NAME = "CmsServerAuth";
        /// <summary>
        /// The default value for width of the page's section.
        /// </summary>
        public const TextWallSectionWidth DEFAULT_TEXT_WALL_SECTION_WIDTH = TextWallSectionWidth._800;
        /// <summary>
        /// The delay for invoking function / action persisting MArkDown editor changes in the specified depository.
        /// </summary>
        public const int MARKDOWN_EDITOR_DELAY_FOR_UPDATING_DEPOSITORY_MILLIS = 2000;
        /// <summary>
        /// The constant defining paddings within a body of an accordion.
        /// </summary>
        public const string ACCORDION_CONTENT_CLASS = "p-0 border-bottom border-dark border-1";
    }
}
