using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Security.IdentityServer.CustomAuthorizationPolicyProvider
{
    public class SecurityLevelAttribute : AuthorizeAttribute
    {
        public SecurityLevelAttribute()
        {

        }
    }
    /// <summary>
    /// create a dynamicaly policy we will use this provider class 
    /// dynamicaly create policy with static enums or static strings
    /// </summary>
    public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach (var customPolicy in DynamicPolicies.GetPolicies())
            {
                if (policyName.StartsWith(customPolicy))
                {
                    var policy = new AuthorizationPolicyBuilder().Build();
                    return Task.FromResult(policy);
                }
            }
            return base.GetPolicyAsync(policyName);
        }
    }
    /// <summary>
    /// types of policies
    /// </summary>
    public static class DynamicPolicies
    {
        public static IEnumerable<string> GetPolicies()
        {
            yield return SecurityLevel;
            yield return Rank;
        }
        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
    }
}
