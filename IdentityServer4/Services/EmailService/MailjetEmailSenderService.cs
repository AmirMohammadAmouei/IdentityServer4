using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace IdentityServer4.Services.EmailService
{
    public class MailjetEmailSenderService : IEmailSender
    {
        private readonly IConfiguration _config;
        public MailJetOptions _mailJetOptions;

        public MailjetEmailSenderService(IConfiguration config)
        {
            _config = config;
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _mailJetOptions = _config.GetSection("MailJet").Get<MailJetOptions>();

            MailjetClient client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "amiramooei@gmail.com")
               .Property(Send.FromName, "amir amooei")
               .Property(Send.Subject, "Test Email!")
               .Property(Send.TextPart, "this a test email in aps.net core identity project")
               .Property(Send.HtmlPart, htmlMessage)
               .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
                   });
            await client.PostAsync(request);
            //if (response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
            //    Console.WriteLine(response.GetData());
            //}
            //else
            //{
            //    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            //    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            //    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            //}
        }


    }
}
