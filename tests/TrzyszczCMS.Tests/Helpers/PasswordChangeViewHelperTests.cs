using NUnit.Framework;
using TrzyszczCMS.Core.Shared.Enums;

namespace TrzyszczCMS.Client.Helpers.Tests
{
    [TestFixture]
    public class PasswordChangeViewHelperTests
    {
        [TestCase(null, "a", "a", "Exceptions MUSTN'T be thrown for 1st argument as null.")]
        [TestCase("a", null, "a", "Exceptions MUSTN'T be thrown for 2nd argument as null.")]
        [TestCase("a", "a", null, "Exceptions MUSTN'T be thrown for 3rd argument as null.")]
        [TestCase("", "a", "a",   "Exceptions MUSTN'T be thrown for 1st argument as string.Empty.")]
        [TestCase("a", "", "a",   "Exceptions MUSTN'T be thrown for 2nd argument as string.Empty.")]
        [TestCase("a", "a", "",   "Exceptions MUSTN'T be thrown for 3rd argument as string.Empty.")]
        public void Test_CheckPasswordsForChangeBeforeSending_ThrowExceptions(string oldPassword, string newPassword, string newPasswordRepeated, string comment)
        {
            Assert.DoesNotThrow(() => PasswordChangeViewHelper.CheckPasswordsForChangeBeforeSending(oldPassword, newPassword, newPasswordRepeated), comment);
        }

        [TestCase("abcd", "",     "",     PasswordNotChangedReason.NotAllDataProvided, "MUST be NotAllDataProvided")]
		[TestCase("",     "abcd", "",     PasswordNotChangedReason.NotAllDataProvided, "MUST be NotAllDataProvided")]
		[TestCase("",     "",     "abcd", PasswordNotChangedReason.NotAllDataProvided, "MUST be NotAllDataProvided")]
		[TestCase("",     "abcd", "abcd", PasswordNotChangedReason.NotAllDataProvided, "MUST be NotAllDataProvided")]
		[TestCase("abcd", "",     "abcd", PasswordNotChangedReason.NotAllDataProvided, "MUST be NotAllDataProvided")]
		[TestCase("abcd", "abcd", "",     PasswordNotChangedReason.NotAllDataProvided, "MUST be NotAllDataProvided")]
		[TestCase("abcd", "abcd", "abcd", PasswordNotChangedReason.NewPasswordEqualsOldPassword, "MUST be NewPass == OldPass")]
		[TestCase("abcd", "ZAQ12wsxCDE34RFV", "ZAQ12wsxCDE34RFV", PasswordNotChangedReason.NewPasswordNotComplexEnough, "MUST be NewPass not complex enough (no special chars)")]
		[TestCase("abcd", "zaq!@wsxcde34rfv", "zaq!@wsxcde34rfv", PasswordNotChangedReason.NewPasswordNotComplexEnough, "MUST be NewPass not complex enough (no uppercase chars)")]
		[TestCase("abcd", "ZAQ!@WSXCDE34RFV", "ZAQ!@WSXCDE34RFV", PasswordNotChangedReason.NewPasswordNotComplexEnough, "MUST be NewPass not complex enough (no lowercase chars)")]
		[TestCase("abcd", "ZAQ!@wsxcde#$rfv", "ZAQ!@wsxcde#$rfv", PasswordNotChangedReason.NewPasswordNotComplexEnough, "MUST be NewPass not complex enough (no numbers)")]
		[TestCase("abcd", "ZAQ!2wsxCDE#4rfv", "ZAQ12wsxCDE34rfv", PasswordNotChangedReason.RepeatedPasswordInvalid, "Passwords are DIFFERENT (NewPass != NewPassRepeat)")]
		[TestCase("abcd", "ZAQ!2wsxCDE#4rfv", "ZAQ!2wsxCDE#4rfv", null, "MUST be OK (null value of error)")]
		[TestCase("abcd", "ZAQ!2wsx", "ZAQ!2wsx", PasswordNotChangedReason.NewPasswordNotComplexEnough, "MUST be NewPass not complex enough (not long)")]
        public void Test_CheckPasswordsForChangeBeforeSending_CheckReturnedValueEquality(string oldPassword, string newPassword, string newPasswordRepeated, PasswordNotChangedReason? result, string comment)
        {
            Assert.AreEqual(PasswordChangeViewHelper.CheckPasswordsForChangeBeforeSending(oldPassword, newPassword, newPasswordRepeated), result, comment);
        }
    }
}