using System.Collections.Generic;
using System.Linq;
using TrzyszczCMS.Client.Helpers;

namespace TrzyszczCMS.Client.Other.MarkDown.Strategies
{
    public class LeftCharSuffixFormatStrategy : ILeftSuffixFormatStrategy
    {
        private readonly int _repeatability;
        private readonly char _charSuffix;

        public LeftCharSuffixFormatStrategy(string insertedSampleWhenNoTextSelected, int repeatability, char charSuffix)
        {
            this.InsertedSampleWhenNoTextSelected = insertedSampleWhenNoTextSelected;
            this._repeatability = repeatability;
            this._charSuffix = charSuffix;
        }


        public string InsertedSampleWhenNoTextSelected { get; private set; }
        public IEnumerable<int> CountSuffixesLengthsForSelectedText(IEnumerable<string> selectedTextBlock)
        {
            return selectedTextBlock.Select(s =>
                s.TakeWhile(c => c == _charSuffix).Count()
            );
        }
        public int GetRemainingSuffixesQuantity(IEnumerable<int> textSuffixesLengths)
        {
            if (_repeatability == 0)
            {
                return textSuffixesLengths.Max() == 0 ? 1 : 0;
            }
            else
            {
                return (textSuffixesLengths.Max() + 1) % (_repeatability + 1);
            }
        }

        public string GetNewSuffix(int remainingSuffixes) =>
            StringHelper.ProduceCharSequence(_charSuffix, remainingSuffixes);

        public IEnumerable<string> GetNewSuffixedText(IEnumerable<string> textWithoutSuffixes, IEnumerable<int> textSuffixesLengths, string newSuffix) =>
            textWithoutSuffixes.Select(i => $"{newSuffix}{i}");
    }
}
