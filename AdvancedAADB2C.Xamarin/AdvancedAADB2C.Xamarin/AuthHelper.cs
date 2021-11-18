using Microsoft.Identity.Client;
using System.Linq;

namespace AdvancedAADB2C.Xamarin
{
    static class AuthHelper
    {
        internal static Identity GetIdentityFromAuthenticationResult(AuthenticationResult result)
        {
            if (result == null || result.ClaimsPrincipal == null || !result.ClaimsPrincipal.Claims.Any())
            {
                return null;
            }

            return new Identity()
            {
                Id = result.ClaimsPrincipal.Claims.FirstOrDefault(e => e.Type == "oid")?.Value,
                Name = result.ClaimsPrincipal.Claims.FirstOrDefault(e => e.Type == "name")?.Value,
                Email = result.ClaimsPrincipal.Claims.FirstOrDefault(e => e.Type == "emails")?.Value
            };
        }
    }
}
