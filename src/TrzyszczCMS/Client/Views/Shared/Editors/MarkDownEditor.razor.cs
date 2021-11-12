using Core.Application.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data;
using TrzyszczCMS.Client.Data.Enums;
using TrzyszczCMS.Client.Data.Enums.Extensions;
using TrzyszczCMS.Client.Data.Model.JSInterop;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Views.Shared.Editors
{
    public partial class MarkDownEditor
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

        #region Fields
        private string markDownCode = string.Empty;
        private ToggledMarkDownViewMode editorWorkMode = ToggledMarkDownViewMode.EditorWithPreview;
        #endregion

        #region Properties
        private string CssClassesForEditor =>
            CssClassesHelper.ClassesForMarkDownEditor(editorWorkMode).EditorCss;
        private string CssClassesForPreview =>
            CssClassesHelper.ClassesForMarkDownEditor(editorWorkMode).PreviewCss;
        private bool EditorButtonsDisabled =>
            editorWorkMode == ToggledMarkDownViewMode.Preview;
        #endregion

        #region Init
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        #endregion

        #region Handles
        private void OnInputChanged(ChangeEventArgs args) =>
            markDownCode = args.Value.ToString();

        private void ToggleView()
        {
            editorWorkMode = editorWorkMode.NextValue();
        }

        private async Task AddFormattingAsterisks(AsteriskSuffixFormat format)
        {
            var range = await this.GetSelectionRangeAsync();
            if (!range.IsTextSelected())
            {
                return;
            }

            byte foundAsterisks = this.FindAsterisksInMarkDownCode(range);
            int countOfAsterisksToInsert = this.ResolveCountOfAsterisksToInsert(format, foundAsterisks);
            
            if (countOfAsterisksToInsert < 0)
            {
                var countOfAsterisksToRemove = countOfAsterisksToInsert * -1;
                markDownCode = markDownCode.Remove(range.End,  countOfAsterisksToRemove)
                                           .Remove(range.Start-countOfAsterisksToRemove, countOfAsterisksToRemove);

                await this.SelectTextRangeAsync(range.Start - countOfAsterisksToRemove, range.End - countOfAsterisksToRemove);
            }
            else
            {
                markDownCode = markDownCode.Insert(range.End,   StringHelper.ProduceCharSequence('*', countOfAsterisksToInsert))
                                           .Insert(range.Start, StringHelper.ProduceCharSequence('*', countOfAsterisksToInsert));

                await this.SelectTextRangeAsync(range.Start + countOfAsterisksToInsert, range.End + countOfAsterisksToInsert);
            }
        }

        private async Task AddFormattingSuffixes([NotNull] string suffix)
        {
            var range = await this.GetSelectionRangeAsync();
            if (!range.IsTextSelected())
            {
                return;
            }

            markDownCode += " ";
            // --- Formatting ---
            bool reverse = false;

            if (range.End + suffix.Length < markDownCode.Length &&
                markDownCode.Substring(range.End, suffix.Length) == suffix && // Check for suffix.
                markDownCode[range.End + suffix.Length] != suffix[0])         // Check for next char.
            {
                markDownCode = markDownCode.Remove(range.End, suffix.Length);
                reverse = true;
            }
            if (range.Start > 0 &&
                markDownCode.Substring(range.Start - suffix.Length, suffix.Length) == suffix &&
                (range.Start - suffix.Length - 1 < 0 || markDownCode[range.Start - suffix.Length - 1] != suffix[0]))
            {
                markDownCode = markDownCode.Remove(range.Start - suffix.Length, suffix.Length);
                reverse = true;
            }

            if (!reverse)
            {
                markDownCode = this.markDownCode.Insert(range.End, suffix)
                                                .Insert(range.Start, suffix);
            }
            // ------------------
            markDownCode = markDownCode.Remove(markDownCode.Length - 1, 1);


            var selectionStartingIndex = range.Start + ((reverse ? -1 : 1)  * suffix.Length);
            await this.SelectTextRangeAsync(selectionStartingIndex, selectionStartingIndex + range.GetLength());
        }

        private async Task AddFormattingSuffixesLeft(char? charSuffix = null, string stringSuffix = null, byte charSuffixRepeatability = 0)
        {
            // TODO: IMPORTANT    Move formatting markdown to another class with strategy support.   IMPORTANT
            var range = await this.GetSelectionRangeAsync();
            if (!range.IsTextSelected())
            {
                string insertedSample = string.Empty;
                if (charSuffix.HasValue)
                {
                    if (charSuffix == '*')
                    {
                        insertedSample = Constants.MarkDown.UNORDERED_LIST_SAMPLE;
                    }
                    else if (charSuffix == '>')
                    {
                        insertedSample = Constants.MarkDown.QUOTE_SAMPLE;
                    }
                }
                else if (!string.IsNullOrEmpty(stringSuffix))
                {
                    insertedSample = Constants.MarkDown.CHECKLIST_SAMPLE;
                }
                else
                {
                    insertedSample = Constants.MarkDown.ORDERED_LIST_SAMPLE;
                }
                markDownCode = markDownCode.Insert(range.Start, insertedSample);
                await this.SelectTextRangeAsync(range.Start + insertedSample.Length);
                return;
            }

            var selectedTextBlock = markDownCode.Substring(range.Start, range.GetLength())
                                                .Split(Environment.NewLine)
                                                .Select(i => i.TrimStart());

            IEnumerable<int> textSuffixesLengths;
            if (charSuffix.HasValue)
            {
                textSuffixesLengths = selectedTextBlock.Select(s =>
                    s.TakeWhile(c => c == charSuffix).Count()
                );
            }
            else if (!string.IsNullOrEmpty(stringSuffix))
            {
                textSuffixesLengths = selectedTextBlock.Select(s =>
                    stringSuffix.Length < s.Length && s.Substring(0, stringSuffix.Length) == stringSuffix ? stringSuffix.Length : 0
                );
            }
            else
            {
                textSuffixesLengths = selectedTextBlock.Select(s =>
                    s.TakeWhile(c => char.IsNumber(c) || c == '.').Count()
                );
            }

            var textWithoutSuffixes = selectedTextBlock.Zip(textSuffixesLengths)
                                                       .Select(i => i.First[(i.Second == 0 ? 0 : i.Second + 1)..]);

            int remainingSuffixes = 0;
            if (charSuffixRepeatability == 0)
            {
                remainingSuffixes = textSuffixesLengths.Max() == 0 ? 1 : 0;
            }
            else
            {
                remainingSuffixes = (textSuffixesLengths.Max() + 1) % (charSuffixRepeatability + 1);
            }

            string newSuffix = string.Empty;
            if (charSuffix.HasValue)
            {
                newSuffix = StringHelper.ProduceCharSequence(charSuffix.Value, remainingSuffixes);
            }
            else if (!string.IsNullOrEmpty(stringSuffix))
            {
                newSuffix = remainingSuffixes > 0 ? stringSuffix : string.Empty;
            }

            if (newSuffix.Length != 0)
            {
                newSuffix += " ";
            }

            string newTextBlock;
            if (charSuffix.HasValue || !string.IsNullOrEmpty(stringSuffix))
            {
                newTextBlock = textWithoutSuffixes.Select(i => $"{newSuffix}{i}").Aggregate(Environment.NewLine).ToString();
            }
            else
            {
                if (textSuffixesLengths.Max() > 0)
                {
                    newTextBlock = textWithoutSuffixes.Aggregate(Environment.NewLine).ToString();
                }
                else
                {
                    newTextBlock = textWithoutSuffixes.Select((i, ix) => $"{ix+1}. {i}").Aggregate(Environment.NewLine).ToString();
                }
            }
            
            markDownCode = markDownCode.Remove(range.Start, range.GetLength())
                                       .Insert(range.Start, newTextBlock);

            await this.SelectTextRangeAsync(range.Start, range.Start + newTextBlock.Length);
        }

        private async Task AddLinkBasedText(LinkBasedContentType type)
        {
            const string urlName = "URL";
            var range = await this.GetSelectionRangeAsync();
            string format = this.GetLinkBasedFormatString(type);

            var description = range.IsTextSelected() ?
                markDownCode.Substring(range.Start, range.GetLength()) :
                "DESCRIPTION";

            var templateBasedText = format.Contains("{0}") && format.Contains("{1}");
            
            string insertText = templateBasedText ?
                string.Format(format, description, urlName) :
                format;
            
            markDownCode = markDownCode.Remove(range.Start, range.GetLength())
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

        #region JSInterop
        private async Task<SelectionRange> GetSelectionRangeAsync() =>
            await this.JSInterop.GetSelectionRangeAsync("txarMarkDownEditedText");
        
        private async Task SelectTextRangeAsync(int index) =>
            await this.JSInterop.SelectTextRangeAsync("txarMarkDownEditedText", index, this.StateHasChanged);

        private async Task SelectTextRangeAsync(int start, int end) =>
            await this.JSInterop.SelectTextRangeAsync("txarMarkDownEditedText", start, end, this.StateHasChanged);
        #endregion

        #region Helpers
        private byte FindAsterisksInMarkDownCode(SelectionRange range)
        {
            byte foundAsterisks = 0;
            for (int testedCount = 1;
                testedCount <= 3 && range.End + testedCount <= markDownCode.Length && range.Start - testedCount >= 0;
                ++testedCount)
            {
                var rightAsterisks = (byte)markDownCode.Substring(range.End, testedCount).Count(i => i == '*');
                var leftAsterisks  = (byte)markDownCode.Substring(range.Start - testedCount, testedCount).Count(i => i == '*');
                // TODO: Implement backward substring.

                if (leftAsterisks == rightAsterisks)
                {
                    foundAsterisks = leftAsterisks;
                }
            }
            return foundAsterisks;
        }

        private int ResolveCountOfAsterisksToInsert(AsteriskSuffixFormat format, byte foundAsterisks)
        {
            switch (format)
            {
                case AsteriskSuffixFormat.Bold:   return 2 * ((foundAsterisks >> 1) % 2 == 0 ? 1 : -1);
                case AsteriskSuffixFormat.Italic: return 1 * (foundAsterisks % 2 == 0 ? 1 : -1);
                default: return 0;
            }
        }

        private string GetLinkBasedFormatString(LinkBasedContentType type)
        {
            switch (type)
            {
                case LinkBasedContentType.Hyperlink:   return Constants.MarkDown.HYPERLINK_FORMAT;
                case LinkBasedContentType.ImageSource: return Constants.MarkDown.IMAGE_SOURCE_FORMAT;
                case LinkBasedContentType.Table:       return Constants.MarkDown.TABLE_BLOCK_SAMPLE;
                default:
                    throw new ArgumentException($"Value of the enum {nameof(LinkBasedContentType)} ({type}) is unhandled.", nameof(type));
            }
        }
        #endregion
    }
}