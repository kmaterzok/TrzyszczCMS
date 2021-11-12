using System.Collections.Generic;
using System.Linq;

namespace TrzyszczCMS.Client.Other.MarkDown.Strategies
{
    public class LeftStringSuffixFormatStrategy : ILeftSuffixFormatStrategy
    {
        private readonly string _stringSuffix;

        public LeftStringSuffixFormatStrategy(string insertedSampleWhenNoTextSelected, string stringSuffix)
        {
            this.InsertedSampleWhenNoTextSelected = insertedSampleWhenNoTextSelected;
            this._stringSuffix = stringSuffix;
        }


        public string InsertedSampleWhenNoTextSelected { get; private set; }
        public IEnumerable<int> CountSuffixesLengthsForSelectedText(IEnumerable<string> selectedTextBlock)
        {
            return selectedTextBlock.Select(s =>
                _stringSuffix.Length < s.Length && s.Substring(0, _stringSuffix.Length) == _stringSuffix ? _stringSuffix.Length : 0
            );
        }
        public int GetRemainingSuffixesQuantity(IEnumerable<int> textSuffixesLengths) =>
            textSuffixesLengths.Max() == 0 ? 1 : 0;

        public string GetNewSuffix(int remainingSuffixes) =>
            remainingSuffixes > 0 ? _stringSuffix : string.Empty;

        public IEnumerable<string> GetNewSuffixedText(IEnumerable<string> textWithoutSuffixes, IEnumerable<int> textSuffixesLengths, string newSuffix) =>
            textWithoutSuffixes.Select(i => $"{newSuffix}{i}");
    }
}
