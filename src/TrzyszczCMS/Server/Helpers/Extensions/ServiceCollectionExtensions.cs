using DAL.Shared.Data;
using Microsoft.Extensions.DependencyInjection;

namespace TrzyszczCMS.Server.Helpers.Extensions
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
            
            services.AddAuthorization(options =>
            {
                foreach (var availablePolicy in availablePolicies)
                {
                    options.AddPolicy(availablePolicy.Value, policy =>
                        policy.RequireClaim(availablePolicy.Value, stringOfTrue)
                    );
                }
            });
        }
    }
}
