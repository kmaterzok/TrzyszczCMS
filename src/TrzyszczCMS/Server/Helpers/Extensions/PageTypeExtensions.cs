using Core.Shared.Enums;
using Core.Shared.Helpers;
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
            var availableTypes = typeof(DAL.Shared.Data.UserPolicies).GetConstants<string>();
            string pageTypeString;
            
            switch (type)
            {
                case PageType.Article:
                case PageType.HomePage: pageTypeString = type.ToString().ToUpper();
                    break;
                case PageType.Post:     pageTypeString = "BLOG_POST";
                    break;
                default:
                    throw ExceptionMaker.Argument.Unsupported(type, nameof(type));
            }
            string operationString = operation.ToString().ToUpper();

            var constantName = $"{pageTypeString}_{operationString}";

            return availableTypes[constantName];
        }
    }
}
