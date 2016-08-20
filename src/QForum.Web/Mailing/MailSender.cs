using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using QForum.Web.Models.AppSettings;
using System.Linq;

namespace QForum.Web.Mailing
{
    public class MailSender : IMailSender
    {
        private readonly IOptions<MailSettings> _settings;

        public MailSender(IOptions<MailSettings> settings)
        {
            _settings = settings;
        }

        public async Task SendAsync(string subject, string body, List<Recipient> recipients = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.Value.SenderName, _settings.Value.SenderEmail));

            if(recipients == null || !recipients.Any())
                recipients = new List<Recipient>
                {
                    new Recipient
                    {
                        Name = _settings.Value.DefaultRecipientName,
                        Email = _settings.Value.DefaultRecipientEmail
                    }
                };

            foreach (var recipient in recipients)
            {
                message.To.Add(new MailboxAddress(recipient.Name, recipient.Email));
            }

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(_settings.Value.SenderEmail, _settings.Value.SenderPassword);

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}