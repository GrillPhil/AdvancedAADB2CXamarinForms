using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AdvancedAADB2C.Backend
{
    public static class Redeem
    {
        [FunctionName("Redeem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "redeem")] HttpRequest req,
            ILogger log)
        {
            var token = req.Query["t"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                var redirectUrl = Auth.GetRedeemUrl(token);
                return new RedirectResult(redirectUrl);
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
