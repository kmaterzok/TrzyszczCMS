using TrzyszczCMS.Core.Infrastructure.Shared.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace TrzyszczCMS.Core.Shared.Helpers.Extensions
{
    /// <summary>
    /// The extension methods for easy
    /// <see cref="IServiceCollection"/> management.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a policy-based authorisation for users in the backend.
        /// </summary>
        /// <param name="services">Instance of services collection</param>
        public static void AddUserPolicyAuthorisation(this IServiceCollection services)
        {
            var availablePolicies = typeof(UserPolicies).GetConstants<string>();
            var stringOfTrue = true.ToString();
            
            services.AddAuthorizationCore(options =>
            {
                foreach (var availablePolicy in availablePolicies)
                {
                    options.AddPolicyDirectly(availablePolicy.Value, availablePolicy.Value);
                }
            });
        }

        #region Helper methods
        /// <summary>
        /// Simplified adding policies with translation of <paramref name="claimName"/> into <paramref name="policyName"/>.
        /// <paramref name="claimName"/>'s value must be bool value stored as string (stored as <c>"True"</c> or <c>"False"</c> string).
        /// </summary>
        /// <param name="options">Authorisation options that let adding new policies</param>
        /// <param name="policyName">Policy name</param>
        /// <param name="claimName">Claim name used in the application</param>
        private static void AddPolicyDirectly(this AuthorizationOptions options, string policyName, string claimName) =>
            options.AddPolicy(policyName, policy => policy.RequireClaim(claimName, true.ToString()));
        #endregion
    }
}
