using Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Services.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SmtpClient
                {
                    Port = _configuration.GetValue<int>(Constant.EMAIL_SENDER_PORT),
                    Host = _configuration.GetValue<string>(Constant.EMAIL_SENDER_HOST),
                    EnableSsl = _configuration.GetValue<bool>(Constant.EMAIL_SENDER_ENABLE_SSL),
                    UseDefaultCredentials = _configuration.GetValue<bool>(Constant.EMAIL_SENDER_USE_DEFAULT_CREENTIALS),
                    Credentials = new NetworkCredential(_configuration.GetValue<string>(Constant.EMAIL_SENDER_EMAIL),
                                                        _configuration.GetValue<string>(Constant.EMAIL_SENDER_PASSWORD))
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_configuration.GetValue<string>(Constant.EMAIL_SENDER_EMAIL)),
                    Subject = subject,
                };
                message.To.Add(new MailAddress(email));
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlMessage, null, MediaTypeNames.Text.Html));

                await client.SendMailAsync(message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
