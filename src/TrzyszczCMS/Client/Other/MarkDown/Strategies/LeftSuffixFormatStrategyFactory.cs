using Core.Shared.Helpers;

namespace TrzyszczCMS.Client.Other.MarkDown.Strategies
{
    /// <summary>
    /// The factory generating suffix strategies for processing MarkDown code.
    /// </summary>
    public static class LeftSuffixFormatStrategyFactory
    {
        /// <summary>
        /// Make a new strategy that precises handling with left-side markups (suffixes)
        /// </summary>
        /// <param name="type">Type of suffix to handle</param>
        /// <returns>Generated strategy</returns>
        public static ILeftSuffixFormatStrategy Make(LeftSuffixType type) => type switch
        {
            LeftSuffixType.Heading       => new LeftCharSuffixFormatStrategy(string.Empty, 6, MarkDownConstants.HEADING_SUFFIX),
            LeftSuffixType.OrderedList   => new LeftOrderedListSuffixFormatStrategy(MarkDownConstants.ORDERED_LIST_SAMPLE),
            LeftSuffixType.UnorderedList => new LeftCharSuffixFormatStrategy(MarkDownConstants.UNORDERED_LIST_SAMPLE, 0, MarkDownConstants.UNORDERED_LIST_SUFFIX),
            LeftSuffixType.Checklist     => new LeftStringSuffixFormatStrategy(MarkDownConstants.CHECKLIST_SAMPLE, MarkDownConstants.CHECKLIST_SUFFIX),
            LeftSuffixType.QuoteBlock    => new LeftCharSuffixFormatStrategy(MarkDownConstants.QUOTE_BLOCK_SAMPLE, 0, MarkDownConstants.QUOTE_BLOCK_SUFFIX),
            _ => throw ExceptionMaker.Argument.Unsupported(type, nameof(type)),
        };
    }
}
