using NUnit.Framework;
using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Server.Data.Enums;

namespace TrzyszczCMS.Server.Helpers.Extensions.Tests
{
    [TestFixture]
    public class PageTypeExtensionsTests
    {
        [TestCase(PageType.Article,  PageOperationType.Creating)]
        [TestCase(PageType.Article,  PageOperationType.Deleting)]
        [TestCase(PageType.Article,  PageOperationType.Editing)]
        [TestCase(PageType.Post,     PageOperationType.Creating)]
        [TestCase(PageType.Post,     PageOperationType.Deleting)]
        [TestCase(PageType.Post,     PageOperationType.Editing)]
        [TestCase(PageType.HomePage, PageOperationType.Editing)]
        public void Test_GetUserPolicyName_NoException(PageType type, PageOperationType operation) =>
            Assert.DoesNotThrow(() => type.GetUserPolicyName(operation), "this operation MUST NOT throw any exception with proper data.");
    }
}