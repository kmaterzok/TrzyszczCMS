using System;

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
        public static ILeftSuffixFormatStrategy Make(LeftSuffixType type)
        {
            switch(type)
            {
                case LeftSuffixType.Heading:
                    return new LeftCharSuffixFormatStrategy(string.Empty, 6, MarkDownConstants.HEADING_SUFFIX);
                case LeftSuffixType.OrderedList:
                    return new LeftOrderedListSuffixFormatStrategy(MarkDownConstants.ORDERED_LIST_SAMPLE);
                case LeftSuffixType.UnorderedList:
                    return new LeftCharSuffixFormatStrategy(MarkDownConstants.UNORDERED_LIST_SAMPLE, 0, MarkDownConstants.UNORDERED_LIST_SUFFIX);
                case LeftSuffixType.Checklist:
                    return new LeftStringSuffixFormatStrategy(MarkDownConstants.CHECKLIST_SAMPLE, MarkDownConstants.CHECKLIST_SUFFIX);
                case LeftSuffixType.QuoteBlock:
                    return new LeftCharSuffixFormatStrategy(MarkDownConstants.QUOTE_BLOCK_SAMPLE, 0, MarkDownConstants.QUOTE_BLOCK_SUFFIX);
                default:
                    throw new ArgumentException($"Unsupported value {nameof(type)} of enum {nameof(LeftSuffixType)}", nameof(type));
            }
        }
    }
}
