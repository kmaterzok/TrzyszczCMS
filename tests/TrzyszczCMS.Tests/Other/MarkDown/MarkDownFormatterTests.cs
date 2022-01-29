using NUnit.Framework;
using TrzyszczCMS.Client.Other.MarkDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Data.Model.JSInterop;
using TrzyszczCMS.Client.Other.MarkDown.Strategies;

namespace TrzyszczCMS.Client.Other.MarkDown.Tests
{
    [TestFixture]
    public class MarkDownFormatterTests
    {
        [TestCase(AsteriskSuffixFormat.Bold,   "test",   "**test**",   0, 4, 2, 6)]
        [TestCase(AsteriskSuffixFormat.Bold,   " test ", " **test** ", 1, 5, 3, 7)]
        [TestCase(AsteriskSuffixFormat.Bold,   "test ",  "**test** ",  0, 4, 2, 6)]
        [TestCase(AsteriskSuffixFormat.Bold,   " test",  " **test**",  1, 5, 3, 7)]
        [TestCase(AsteriskSuffixFormat.Italic, "test",   "*test*",   0, 4, 1, 5)]
        [TestCase(AsteriskSuffixFormat.Italic, " test ", " *test* ", 1, 5, 2, 6)]
        [TestCase(AsteriskSuffixFormat.Italic, "test ",  "*test* ",  0, 4, 1, 5)]
        [TestCase(AsteriskSuffixFormat.Italic, " test",  " *test*",  1, 5, 2, 6)]
        public void Test_AddFormattingAsterisks(
            AsteriskSuffixFormat suffixFormat,
            string input, string output,
            int scopeStart, int scopeEnd,
            int afterFormatStart, int afterFormatEnd)
        { // --------------------------------------------
            int selectedStart = 0, selectedEnd = 0;
            var formatter = new MarkDownFormatter();
            formatter.MarkDownCode = input;
            formatter.GetSelectionRangeAsync += async ()       => await Task.FromResult(new SelectionRange(scopeStart, scopeEnd));
            formatter.SelectTextRangeAsync   += async (si, ei) => { selectedStart = si; selectedEnd = ei; await Task.CompletedTask; };
            formatter.SelectTextIndexAsync   += async ix       => { selectedStart = ix; selectedEnd = ix; await Task.CompletedTask; };

            Task.Run(() => formatter.AddFormattingAsterisks(suffixFormat)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, output,  "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, afterFormatStart, "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd,   afterFormatEnd,   "Selected end ISN'T equal to the expected one.");

            formatter.GetSelectionRangeAsync = null;
            formatter.GetSelectionRangeAsync += async () => await Task.FromResult(new SelectionRange(afterFormatStart, afterFormatEnd));
            Task.Run(() => formatter.AddFormattingAsterisks(suffixFormat)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, input, "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, scopeStart,     "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd,   scopeEnd,       "Selected end ISN'T equal to the expected one.");
        }




        [TestCase("!@$%^&", "test",   "!@$%^&test!@$%^&",   0, 4, 6, 10)]
        [TestCase("!@$%^&", " test ", " !@$%^&test!@$%^& ", 1, 5, 7, 11)]
        [TestCase("!@$%^&", "test ",  "!@$%^&test!@$%^& ",  0, 4, 6, 10)]
        [TestCase("!@$%^&", " test",  " !@$%^&test!@$%^&",  1, 5, 7, 11)]
        public void Test_AddFormattingSuffixes(
            string customSuffix,
            string input, string output,
            int scopeStart, int scopeEnd,
            int afterFormatStart, int afterFormatEnd)
        { // --------------------------------------------
            int selectedStart = 0, selectedEnd = 0;
            var formatter = new MarkDownFormatter();
            formatter.MarkDownCode = input;
            formatter.GetSelectionRangeAsync += async () => await Task.FromResult(new SelectionRange(scopeStart, scopeEnd));
            formatter.SelectTextRangeAsync += async (si, ei) => { selectedStart = si; selectedEnd = ei; await Task.CompletedTask; };
            formatter.SelectTextIndexAsync += async ix => { selectedStart = ix; selectedEnd = ix; await Task.CompletedTask; };

            Task.Run(() => formatter.AddFormattingSuffixes(customSuffix)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, output, "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, afterFormatStart, "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd, afterFormatEnd, "Selected end ISN'T equal to the expected one.");

            formatter.GetSelectionRangeAsync = null;
            formatter.GetSelectionRangeAsync += async () => await Task.FromResult(new SelectionRange(afterFormatStart, afterFormatEnd));
            Task.Run(() => formatter.AddFormattingSuffixes(customSuffix)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, input, "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, scopeStart, "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd, scopeEnd, "Selected end ISN'T equal to the expected one.");
        }




        [TestCase(LeftSuffixType.Checklist,     "te\nst",   "* [ ] te\n* [ ] st",    0, 5, 0, 17)]
        [TestCase(LeftSuffixType.OrderedList,   "te\nst",   "1. te\n2. st",          0, 5, 0, 11)]
        [TestCase(LeftSuffixType.UnorderedList, "te\nst",   "* te\n* st",            0, 5, 0, 9)]
        [TestCase(LeftSuffixType.QuoteBlock,    "te\nst",   "> te\n> st",            0, 5, 0, 9)]
        public void Test_AddFormattingSuffixesLeft_AllExceptHeading(
            LeftSuffixType leftSuffix,
            string input, string output,
            int scopeStart, int scopeEnd,
            int afterFormatStart, int afterFormatEnd)
        { // --------------------------------------------
            input  = input.Replace( "\n", Environment.NewLine);
            output = output.Replace("\n", Environment.NewLine);
            int selectedStart = 0, selectedEnd = 0;
            var formatter = new MarkDownFormatter();
            formatter.MarkDownCode = input;
            formatter.GetSelectionRangeAsync += async ()       => await Task.FromResult(new SelectionRange(scopeStart, scopeEnd));
            formatter.SelectTextRangeAsync   += async (si, ei) => { selectedStart = si; selectedEnd = ei; await Task.CompletedTask; };
            formatter.SelectTextIndexAsync   += async ix       => { selectedStart = ix; selectedEnd = ix; await Task.CompletedTask; };

            Task.Run(() => formatter.AddFormattingSuffixesLeft(leftSuffix)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, output,  "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, afterFormatStart, "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd,   afterFormatEnd,   "Selected end ISN'T equal to the expected one.");

            formatter.GetSelectionRangeAsync = null;
            formatter.GetSelectionRangeAsync += async () => await Task.FromResult(new SelectionRange(afterFormatStart, afterFormatEnd));
            Task.Run(() => formatter.AddFormattingSuffixesLeft(leftSuffix)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, input, "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, scopeStart,     "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd,   scopeEnd,       "Selected end ISN'T equal to the expected one.");
        }




        [TestCase("test",        "# test",      0,  4, 0,  6)]
        [TestCase("# test",      "## test",     0,  6, 0,  7)]
        [TestCase("## test",     "### test",    0,  7, 0,  8)]
        [TestCase("### test",    "#### test",   0,  8, 0,  9)]
        [TestCase("#### test",   "##### test",  0,  9, 0, 10)]
        [TestCase("##### test",  "###### test", 0, 10, 0, 11)]
        [TestCase("###### test", "test",        0, 11, 0,  4)]
        public void Test_AddFormattingSuffixesLeft_Heading(
            string input, string output,
            int scopeStart, int scopeEnd,
            int afterFormatStart, int afterFormatEnd)
        { // --------------------------------------------
            int selectedStart = 0, selectedEnd = 0;
            var formatter = new MarkDownFormatter();
            formatter.MarkDownCode = input;
            formatter.GetSelectionRangeAsync += async ()       => await Task.FromResult(new SelectionRange(scopeStart, scopeEnd));
            formatter.SelectTextRangeAsync   += async (si, ei) => { selectedStart = si; selectedEnd = ei; await Task.CompletedTask; };
            formatter.SelectTextIndexAsync   += async ix       => { selectedStart = ix; selectedEnd = ix; await Task.CompletedTask; };

            Task.Run(() => formatter.AddFormattingSuffixesLeft(LeftSuffixType.Heading)).Wait();

            Assert.AreEqual(formatter.MarkDownCode, output, "Modified MD code ISN'T equal to the expected one.");
            Assert.AreEqual(selectedStart, afterFormatStart, "Selected start ISN'T equal to the expected one.");
            Assert.AreEqual(selectedEnd, afterFormatEnd, "Selected end ISN'T equal to the expected one.");
        }
    }
}