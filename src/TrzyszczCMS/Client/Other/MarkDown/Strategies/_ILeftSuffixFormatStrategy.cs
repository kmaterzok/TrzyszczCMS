using System.Collections.Generic;

namespace TrzyszczCMS.Client.Other.MarkDown.Strategies
{
    /// <summary>
    /// It determines how <see cref="MarkDownFormatter.AddFormattingSuffixesLeft(ILeftSuffixFormatStrategy)"/>
    /// should work with formatting.
    /// </summary>
    public interface ILeftSuffixFormatStrategy
    {
        /// <summary>
        /// The text inserted when no text is selected
        /// and only the cursor points the place in the text.
        /// </summary>
        string InsertedSampleWhenNoTextSelected { get; }
        /// <summary>
        /// Count lengths of suffixes in the text.
        /// </summary>
        /// <param name="selectedTextBlock">Enumeration of selected text blocks</param>
        /// <returns>Lengths of suffixes</returns>
        IEnumerable<int> CountSuffixesLengthsForSelectedText(IEnumerable<string> selectedTextBlock);
        /// <summary>
        /// Get quanity of suffixes that must be present in the formatted text after formatting.
        /// </summary>
        /// <param name="textSuffixesLengths">Lengths of suffixes in all of the text lines</param>
        /// <returns>Next remainig suffixes quantity</returns>
        int GetRemainingSuffixesQuantity(IEnumerable<int> textSuffixesLengths);
        /// <summary>
        /// Get string representing a new suffix for formatted text for a single line.
        /// </summary>
        /// <param name="remainingSuffixes">Quantity of charracters in a new suffix if the suffix is buildt from characters</param>
        /// <returns>Produced suffix</returns>
        string GetNewSuffix(int remainingSuffixes);
        /// <summary>
        /// Get text for replacement in the markdown code.
        /// </summary>
        /// <param name="textWithoutSuffixes">Lines of text without suffixes</param>
        /// <param name="textSuffixesLengths">Lengths suffixes peer eacfh line in the old text</param>
        /// <param name="newSuffix">New suffix to add if it is the same in the each line of new text</param>
        /// <returns>New lines of formatted text</returns>
        IEnumerable<string> GetNewSuffixedText(IEnumerable<string> textWithoutSuffixes, IEnumerable<int> textSuffixesLengths, string newSuffix);
    }
}
