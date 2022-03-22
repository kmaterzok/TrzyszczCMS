using NUnit.Framework;

namespace TrzyszczCMS.Core.Shared.Helpers.Tests
{
    [TestFixture]
    public class RegexHelperTests
    {
        [Test]
        public void Test_IsValidPageUriName_ThrowingExceptions()
        {
            Assert.DoesNotThrow(() => RegexHelper.IsValidPageUriName(null), "Exception MUSTN'T be thrown at all.");
        }

        [TestCase(null,                     "Nulls are INVALID.")]
        [TestCase("",                       "Empty strings are INVALID.")]
        [TestCase(@"!@#$%^&*()=+[]{};:/?\", "Most special characters are INVALID.")]
        public void Test_IsValidPageUriName_False(string value, string comment)
        {
            Assert.False(RegexHelper.IsValidPageUriName(value), comment);
        }

        [TestCase("qwertyuiopasdfghjklzxcvbnm", "Lowercase letters are VALID.")]
        [TestCase("QWERTYUIOPASDFGHJKLZXCVBNM", "Uppercase letters are VALID.")]
        [TestCase("1234567890",                 "Digits are VALID.")]
        [TestCase("-",                          "Dashes are VALID.")]
        public void Test_IsValidPageUriName_True(string value, string comment)
        {
            Assert.True(RegexHelper.IsValidPageUriName(value), comment);
        }




        [TestCase(null,                         "Null are INVALID.")]
        [TestCase(null,                         "Null are INVALID.")]
        [TestCase("",                           "Empty strings are INVALID.")]
        [TestCase("QWERTYUIOPASDFGHJKLZXCVBNM", "Uppercase letters are INVALID.")]
        [TestCase(@"!@#$%^&*()=+[]{};:/?\",     "Most special characters are INVALID.")]
        public void Test_IsValidUserName_False(string value, string comment)
        {
            Assert.False(RegexHelper.IsValidUserName(value), comment);
        }

        [TestCase("qwertyuiopasdfghjklzxcvbnm", "Lowercase letters are VALID.")]
		[TestCase("1234567890",                 "Digits are VALID.")]
		[TestCase("-", "Dashes are VALID.")]
		[TestCase("_", "Dashes are VALID.")]
        public void Test_IsValidUserName_True(string value, string comment)
        {
            Assert.True(RegexHelper.IsValidUserName(value), comment);
        }

        [Test]
        public void Test_IsValidUserName_ThrowingExceptions()
        {
            Assert.DoesNotThrow(() => RegexHelper.IsValidUserName(null));
        }
    }
}