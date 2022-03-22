using NUnit.Framework;
using System;

namespace TrzyszczCMS.Core.Server.Helpers.Tests
{
    [TestFixture()]
    public class FilterDataParserTests
    {
        [TestCase("2022-02-02 - 2022-03-03", "2022-02-02 00:00:00", "2022-03-03 23:59:59")]
        [TestCase(">= 2022-02-02", "2022-02-02 00:00:00", null)]
        [TestCase("<= 2022-03-03", null, "2022-03-03 23:59:59")]
        [TestCase("",   null, null)]
        [TestCase(null, null, null)]
        public void Test_ToDateRange_Success(string parsedString, string startTime, string endTime)
        {
            DateTime? startDateTime = string.IsNullOrEmpty(startTime) ?
                null : DateTime.ParseExact(startTime, "yyyy-MM-dd HH:mm:ss", null);
            DateTime? endDateTime   = string.IsNullOrEmpty(endTime) ?
                null : DateTime.ParseExact(endTime,   "yyyy-MM-dd HH:mm:ss", null);

            var range = FilterDataParser.ToDateRange(parsedString);
            Assert.AreEqual(range.Start, startDateTime, "Start values MUST be equal to each other.");
            Assert.AreEqual(range.End,     endDateTime, "End values MUST be equal to each other.");
        }

        [TestCase("2022-02-02 -- 2022-03-03")]
        [TestCase(">== 2022-02-02")]
        [TestCase("<== 2022-03-03")]
        [TestCase("<== 2022-03-03 <== 2022-03-03")]
        [TestCase("random")]
        public void Test_ToDateRange_ThrowArgumentExceptions(string parsedString)
        {
            Assert.Throws<ArgumentException>(() => FilterDataParser.ToDateRange(parsedString), "Start values MUST be equal to each other.");
        }

        [TestCase("<= 2022-03-03-00-00-00")]
        [TestCase("<= random")]
        [TestCase("- - -")]
        public void Test_ToDateRange_ThrowFormatExceptions(string parsedString)
        {
            Assert.Throws<FormatException>(() => FilterDataParser.ToDateRange(parsedString), "Start values MUST be equal to each other.");
        }
    }
}