using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdvancedAADB2C.Backend
{
    static class Auth
    {
        internal static async Task<string> GenerateLinkAsync(string displayName, string email, string hostBase)
        {
            var hostName = Environment.GetEnvironmentVariable("HostName");
            var tenantName = Environment.GetEnvironmentVariable("TenantName");
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var redirectUri = Environment.GetEnvironmentVariable("RedirectUri");
            var expectedRedirect = Environment.GetEnvironmentVariable("ExpectedRedirect");
            var template = Environment.GetEnvironmentVariable("GenLinkTemplate");
            var nonce = Guid.NewGuid().ToString("n");

            var requestUrl = string.Format(template, hostName, tenantName, clientId, nonce, redirectUri, email, displayName);

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = false;

            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(requestUrl);

            client.Dispose();

            var location = response.Headers.Location.ToString();

            if (response.StatusCode == System.Net.HttpStatusCode.Redirect && location.StartsWith(expectedRedirect))
            {
                var idToken = location.Substring(expectedRedirect.Length);
                return $"{hostBase}redeem?t={idToken}";
            }
            else
            {
                throw new Exception();
            }
        }

        internal static string GetRedeemUrl(string token)
        {
            var hostName = Environment.GetEnvironmentVariable("HostName");
            var tenantName = Environment.GetEnvironmentVariable("TenantName");
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var finalUrl = Environment.GetEnvironmentVariable("FinalUrl");
            var redeemTemplate = Environment.GetEnvironmentVariable("RedeemTemplate");
            var nonce = Guid.NewGuid().ToString("n");

            return string.Format(redeemTemplate, hostName, tenantName, clientId, nonce, finalUrl, token);
        }
    }
}
