using NUnit.Framework;
using System;

namespace Core.Shared.Helpers.Extensions.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [TestCase("2022-02-02 00:00:00", "2022-02-02 23:59:59", "")]
        [TestCase("2022-02-02 01:00:00", "2022-02-02 23:59:59", "")]
        [TestCase("2022-02-02 23:59:59", "2022-02-02 23:59:59", "")]
        public void Test_GetWithMaxHour(string input, string output, string comment)
        {
            var inputDateTime  = DateTime.ParseExact(input,  "yyyy-MM-dd HH:mm:ss", null);
            var outputDateTime = DateTime.ParseExact(output, "yyyy-MM-dd HH:mm:ss", null);
            Assert.AreEqual(inputDateTime.GetWithMaxHour(), outputDateTime, comment);
        }
    }
}