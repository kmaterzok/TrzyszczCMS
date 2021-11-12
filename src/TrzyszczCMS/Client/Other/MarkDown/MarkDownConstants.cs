namespace TrzyszczCMS.Client.Other.MarkDown
{
    /// <summary>
    /// Constants and data important for making Markdown code
    /// </summary>
    public static class MarkDownConstants
    {
        public const char HEADING_SUFFIX = '#';
        public const char UNORDERED_LIST_SUFFIX = '*';
        public const char QUOTE_BLOCK_SUFFIX = '>';

        public const string CHECKLIST_SUFFIX = "* [ ]";
        public const string STRIKETHROUGH_TEXT_SUFFIX = "~~";
        public const string CODE_TEXT_SUFFIX = "```";
        public const string HYPERLINK_FORMAT = "[{0}]({1})";
        public const string IMAGE_SOURCE_FORMAT = "![{0}]({1})";
        public const string TABLE_BLOCK_SAMPLE = "\n|Header  |Header  |Header  |" +
                                                        "\n|--------|--------|--------|" +
                                                        "\n|Text 1  |Text 2  |Text 3  |" +
                                                        "\n|Text 4  |Text 5  |Text 6  |\n";

        public const string QUOTE_BLOCK_SAMPLE = "\n> Text\n";
        public const string UNORDERED_LIST_SAMPLE = "\n* Text\n* Text\n* Text\n";
        public const string ORDERED_LIST_SAMPLE = "\n1. Text\n2. Text\n3. Text\n";
        public const string CHECKLIST_SAMPLE = "\n* [ ] Text\n* [ ] Text\n* [ ] Text\n";
    }
}
