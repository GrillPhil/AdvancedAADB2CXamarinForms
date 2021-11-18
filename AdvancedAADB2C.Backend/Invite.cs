using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AdvancedAADB2C.Backend
{
    public static class Invite
    {
        [FunctionName("Invite")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "invite")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var email = data?.email?.Value;
            var name = data?.name?.Value;

            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(name))
            {
                var inviteLink = await Auth.GenerateLinkAsync(name, email, RequestHelper.GetBase(req));
                var fromName = "Awesome App";
                var to = email;
                var subject = "Complete your registration";
                var body = "Click here to accept the invite and complete your registration: " + inviteLink;

                Mail.Send(fromName, to, subject, body);

                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
