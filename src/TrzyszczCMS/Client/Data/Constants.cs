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
        /// Constants and data important for making Markdown code
        /// </summary>
        public static class MarkDown
        {
            public const char   HEADING_SUFFIX        = '#';
            public const char   UNORDERED_LIST_SUFFIX = '*';
            public const char   QUOTE_BLOCK_PART      = '>';

            public const string CHECKLIST_POINT_CHECKED   = "* [ ]";
            public const string STRIKETHROUGH_TEXT_SUFFIX = "~~";
            public const string CODE_TEXT_SUFFIX          = "```";
            public const string HYPERLINK_FORMAT          = "[{0}]({1})";
            public const string IMAGE_SOURCE_FORMAT       = "![{0}]({1})";
            public const string TABLE_BLOCK_SAMPLE        = "\n|Header  |Header  |Header  |" +
                                                            "\n|--------|--------|--------|" +
                                                            "\n|Text 1  |Text 2  |Text 3  |" +
                                                            "\n|Text 4  |Text 5  |Text 6  |\n";

            public const string          QUOTE_SAMPLE     = "\n> Text\n";
            public const string UNORDERED_LIST_SAMPLE     = "\n* Text\n* Text\n* Text\n";
            public const string   ORDERED_LIST_SAMPLE     = "\n1. Text\n2. Text\n3. Text\n";
            public const string      CHECKLIST_SAMPLE     = "\n* [ ] Text\n* [ ] Text\n* [ ] Text\n";
        }
    }
}
