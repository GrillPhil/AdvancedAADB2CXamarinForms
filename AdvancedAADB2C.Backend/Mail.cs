using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AdvancedAADB2C.Backend
{
    static class Mail
    {
        internal static void Send(string fromName, string to, string subject, string body, bool isHtml = false)
        {
            var smtpUsername = Environment.GetEnvironmentVariable("SmtpUsername");
            var smtpPassword = Environment.GetEnvironmentVariable("SmtpPassword");
            var smtpHost = Environment.GetEnvironmentVariable("SmtpHost");
            var smtpPort = Environment.GetEnvironmentVariable("SmtpPort");

            var from = new MailAddress(smtpUsername, fromName, Encoding.UTF8);
            var message = new MailMessage();
            message.From = from;
            message.To.Add(to);
            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = isHtml;
            message.HeadersEncoding = Encoding.UTF8;

            using (var client = new SmtpClient(smtpHost, int.Parse(smtpPort)))
            {
                client.DeliveryFormat = SmtpDeliveryFormat.International;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                client.Send(message);
            }
        }
    }
}
