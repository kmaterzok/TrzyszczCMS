using NUnit.Framework;

namespace Core.Shared.Helpers.Extensions.Tests
{
    [TestFixture()]
    public class StringExtensionsTests
    {
        [TestCase("abcdefgh", "abcdefgh",  8)]
        [TestCase("abcdefgh", "abcde",     5)]
        [TestCase("abcdefgh", "abcdefgh", 20)]
        public void Test_Truncate(string input, string output, int maxLength) =>
            Assert.AreEqual(input.Truncate(maxLength), output, "The output IS NOT equal to the expected one.");
    }
}