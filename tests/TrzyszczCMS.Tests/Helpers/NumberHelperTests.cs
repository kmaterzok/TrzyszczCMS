using NUnit.Framework;

namespace TrzyszczCMS.Core.Application.Helpers.Tests
{
    [TestFixture]
    public class NumberHelperTests
    {
        [TestCase(10, 20, 10)]
        [TestCase(20, 20, 20)]
        [TestCase(20, 10, 10)]
        public void ValueOrMaxTest(int value, int max, int expected)
        {
            Assert.AreEqual(NumberHelper.ValueOrMax(value, max), expected, "The returned value ISN'T equal to expected one.");
        }
    }
}