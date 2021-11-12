using System.Collections.Generic;
using System.Linq;

namespace TrzyszczCMS.Client.Other.MarkDown.Strategies
{
    public class LeftOrderedListSuffixFormatStrategy : ILeftSuffixFormatStrategy
    {
        public LeftOrderedListSuffixFormatStrategy(string insertedSampleWhenNoTextSelected)
        {
            this.InsertedSampleWhenNoTextSelected = insertedSampleWhenNoTextSelected;
        }

        public string InsertedSampleWhenNoTextSelected { get; private set; }
        public IEnumerable<int> CountSuffixesLengthsForSelectedText(IEnumerable<string> selectedTextBlock)
        {
            return selectedTextBlock.Select(s =>
                s.TakeWhile(c => char.IsNumber(c) || c == '.').Count()
            );
        }
        public int GetRemainingSuffixesQuantity(IEnumerable<int> textSuffixesLengths) =>
            textSuffixesLengths.Max() == 0 ? 1 : 0;

        public string GetNewSuffix(int remainingSuffixes) => string.Empty;

        public IEnumerable<string> GetNewSuffixedText(IEnumerable<string> textWithoutSuffixes, IEnumerable<int> textSuffixesLengths, string newSuffix)
        {
            if (textSuffixesLengths.Max() > 0)
            {
                return textWithoutSuffixes;
            }
            else
            {
                return textWithoutSuffixes.Select((i, ix) => $"{ix + 1}. {i}");
            }
        }
    }
}
