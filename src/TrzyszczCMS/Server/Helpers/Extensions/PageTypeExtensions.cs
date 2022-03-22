using TrzyszczCMS.Core.Shared.Enums;
using TrzyszczCMS.Core.Shared.Helpers;
using TrzyszczCMS.Core.Shared.Helpers.Extensions;
using TrzyszczCMS.Server.Data.Enums;

namespace TrzyszczCMS.Server.Helpers.Extensions
{
    /// <summary>
    /// The extension methods for eased access to values.
    /// </summary>
    public static class PageTypeExtensions
    {
        /// <summary>
        /// Return a value of policy name depending on <paramref name="type"/> and <paramref name="operation"/>.
        /// </summary>
        /// <param name="type">Type of a page</param>
        /// <param name="operation">The desired operation to execute</param>
        /// <returns>Name of the policy stored in the constant.</returns>
        public static string GetUserPolicyName(this PageType type, PageOperationType operation)
        {
            var availableTypes = typeof(TrzyszczCMS.Core.Infrastructure.Shared.Data.UserPolicies).GetConstants<string>();
            string pageTypeString = type switch
            {
                PageType.Article
                    or PageType.HomePage => type.ToString().ToUpper(),
                PageType.Post            => "BLOG_POST",
                _ => throw ExceptionMaker.Argument.Unsupported(type, nameof(type)),
            };
            string operationString = operation.ToString().ToUpper();

            var constantName = $"{pageTypeString}_{operationString}";

            return availableTypes[constantName];
        }
    }
}
