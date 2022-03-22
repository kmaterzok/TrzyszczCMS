using TrzyszczCMS.Core.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;
using TrzyszczCMS.Client.Data.Model.JSInterop;
using TrzyszczCMS.Client.Helpers;
using TrzyszczCMS.Client.Other.MarkDown.Strategies;

namespace TrzyszczCMS.Client.Other.MarkDown
{
    #region Enums
    /// <summary>
    /// Types of formatting that can be done with asterisks
    /// </summary>
    public enum AsteriskSuffixFormat
    {
        Bold, Italic
    }
    /// <summary>
    /// Types of link based content based on similar MarkDown markups.
    /// </summary>
    public enum LinkBasedContentType
    {
        Hyperlink,
        ImageSource,
        Table
    }
    #endregion

    #region Delegates
    public delegate Task<SelectionRange> GetSelectionRangeHandler();
    public delegate Task SelectTextRangeHandler(int startIndex, int endIndex);
    public delegate Task SelectTextIndexHandler(int index);
    #endregion

    /// <summary>
    /// It formats and applies formatting changes for MarkDown text.
    /// </summary>
    public class MarkDownFormatter
    {
        #region Properties
        private string _markDownCode;
        public string MarkDownCode
        {
            get => _markDownCode;
            set
            {
                _markDownCode = value;
                OnChangeContent?.Invoke(value);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Fired when the MarkDown content was changed.
        /// </summary>
        public event ChangeContentHandler<string> OnChangeContent;
        #endregion

        #region JSInterop textarea handling
        public GetSelectionRangeHandler GetSelectionRangeAsync { get; set; }
        public SelectTextRangeHandler SelectTextRangeAsync { get; set; }
        public SelectTextIndexHandler SelectTextIndexAsync { get; set; }
        #endregion

        #region Methods
        public async Task AddFormattingAsterisks(AsteriskSuffixFormat format)
        {
            var range = await this.GetSelectionRangeAsync.Invoke();
            if (!range.IsTextSelected())
            {
                return;
            }

            byte foundAsterisks = this.FindAsterisksInMarkDownCode(range);
            int countOfAsterisksToInsert = this.ResolveCountOfAsterisksToInsert(format, foundAsterisks);

            if (countOfAsterisksToInsert < 0)
            {
                var countOfAsterisksToRemove = countOfAsterisksToInsert * -1;
                MarkDownCode = MarkDownCode.Remove(range.End, countOfAsterisksToRemove)
                                           .Remove(range.Start - countOfAsterisksToRemove, countOfAsterisksToRemove);

                await this.SelectTextRangeAsync(range.Start - countOfAsterisksToRemove, range.End - countOfAsterisksToRemove);
            }
            else
            {
                MarkDownCode = MarkDownCode.Insert(range.End, StringHelper.ProduceCharSequence('*', countOfAsterisksToInsert))
                                           .Insert(range.Start, StringHelper.ProduceCharSequence('*', countOfAsterisksToInsert));

                await this.SelectTextRangeAsync(range.Start + countOfAsterisksToInsert, range.End + countOfAsterisksToInsert);
            }
        }
        
        public async Task AddFormattingSuffixes([NotNull] string suffix)
        {
            var range = await this.GetSelectionRangeAsync();
            if (!range.IsTextSelected())
            {
                return;
            }

            bool reverse = false;

            if (range.End + suffix.Length <= MarkDownCode.Length &&
                MarkDownCode.Substring(range.End, suffix.Length) == suffix &&                                               // Check for suffix.
                (range.End + suffix.Length == MarkDownCode.Length || MarkDownCode[range.End + suffix.Length] != suffix[0])) // Check for next char.
            {
                MarkDownCode = MarkDownCode.Remove(range.End, suffix.Length);
                reverse = true;
            }
            if (range.Start > 0 && range.Start - suffix.Length >= 0 &&
                MarkDownCode.Substring(range.Start - suffix.Length, suffix.Length) == suffix &&
                (range.Start - suffix.Length - 1 < 0 || MarkDownCode[range.Start - suffix.Length - 1] != suffix[0]))
            {
                MarkDownCode = MarkDownCode.Remove(range.Start - suffix.Length, suffix.Length);
                reverse = true;
            }

            if (!reverse)
            {
                MarkDownCode = this.MarkDownCode.Insert(range.End, suffix)
                                                .Insert(range.Start, suffix);
            }
            
            
            var selectionStartingIndex = range.Start + ((reverse ? -1 : 1) * suffix.Length);
            await this.SelectTextRangeAsync(selectionStartingIndex, selectionStartingIndex + range.GetLength());
        }

        public async Task AddFormattingSuffixesLeft(LeftSuffixType type) =>
            await this.AddFormattingSuffixesLeft(LeftSuffixFormatStrategyFactory.Make(type));

        private async Task AddFormattingSuffixesLeft([NotNull] ILeftSuffixFormatStrategy strategy)
        {
            var range = await this.GetSelectionRangeAsync();
            if (!range.IsTextSelected())
            {
                string insertedSample = strategy.InsertedSampleWhenNoTextSelected;
                MarkDownCode = MarkDownCode.Insert(range.Start, insertedSample);
                await this.SelectTextIndexAsync(range.Start + insertedSample.Length);
                return;
            }

            var selectedTextBlock = MarkDownCode.Substring(range.Start, range.GetLength())
                                                .Split(Environment.NewLine)
                                                .Select(i => i.TrimStart());

            IEnumerable<int> textSuffixesLengths = strategy.CountSuffixesLengthsForSelectedText(selectedTextBlock);
            
            var textWithoutSuffixes = selectedTextBlock.Zip(textSuffixesLengths)
                                                       .Select(i => i.First[(i.Second == 0 ? 0 : i.Second + 1)..]);

            int remainingSuffixes = strategy.GetRemainingSuffixesQuantity(textSuffixesLengths);
            
            string newSuffix = strategy.GetNewSuffix(remainingSuffixes);
            
            if (newSuffix.Length != 0)
            {
                newSuffix += " ";
            }

            var newTextBlock = strategy.GetNewSuffixedText(textWithoutSuffixes, textSuffixesLengths, newSuffix)
                                       .Aggregate(Environment.NewLine)
                                       .ToString();

            MarkDownCode = MarkDownCode.Remove(range.Start, range.GetLength())
                                       .Insert(range.Start, newTextBlock);

            await this.SelectTextRangeAsync(range.Start, range.Start + newTextBlock.Length);
        }

        public async Task AddLinkBasedText(LinkBasedContentType type)
        {
            const string urlName = "URL";
            var range = await this.GetSelectionRangeAsync();
            string format = this.GetLinkBasedFormatString(type);

            var description = range.IsTextSelected() ?
                MarkDownCode.Substring(range.Start, range.GetLength()) :
                "DESCRIPTION";

            var templateBasedText = format.Contains("{0}") && format.Contains("{1}");

            string insertText = templateBasedText ?
                string.Format(format, description, urlName) :
                format;

            MarkDownCode = MarkDownCode.Remove(range.Start, range.GetLength())
                                       .Insert(range.Start, insertText);

            var urlIndex = insertText.LastIndexOf(urlName);
            if (templateBasedText)
            {
                await this.SelectTextRangeAsync(range.Start + urlIndex, range.Start + urlIndex + urlName.Length);
            }
            else
            {
                await this.SelectTextRangeAsync(range.Start, range.Start + insertText.Length);
            }
        }
        #endregion

        #region Helpers
        private byte FindAsterisksInMarkDownCode(SelectionRange range)
        {
            byte foundAsterisks = 0;
            for (int testedCount = 1;
                testedCount <= 3 && range.End + testedCount <= MarkDownCode.Length && range.Start - testedCount >= 0;
                ++testedCount)
            {
                var rightAsterisks = (byte)MarkDownCode.Substring(range.End, testedCount).Count(i => i == '*');
                var leftAsterisks = (byte)MarkDownCode.Substring(range.Start - testedCount, testedCount).Count(i => i == '*');

                if (leftAsterisks == rightAsterisks)
                {
                    foundAsterisks = leftAsterisks;
                }
            }
            return foundAsterisks;
        }

        private int ResolveCountOfAsterisksToInsert(AsteriskSuffixFormat format, byte foundAsterisks) => format switch
        {
            AsteriskSuffixFormat.Bold   => 2 * ((foundAsterisks >> 1) % 2 == 0 ? 1 : -1),
            AsteriskSuffixFormat.Italic => 1 * (foundAsterisks % 2 == 0 ? 1 : -1),
            _ => 0,
        };

        private string GetLinkBasedFormatString(LinkBasedContentType type) => type switch
        {
            LinkBasedContentType.Hyperlink   => MarkDownConstants.HYPERLINK_FORMAT,
            LinkBasedContentType.ImageSource => MarkDownConstants.IMAGE_SOURCE_FORMAT,
            LinkBasedContentType.Table       => MarkDownConstants.TABLE_BLOCK_SAMPLE,
            _ => throw ExceptionMaker.Argument.Unsupported(type, nameof(type)),
        };
        #endregion
    }
}
