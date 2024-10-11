using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Franshy.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly IOptions<EmailSettings> emailsettings;

        public EmailSender(IOptions<EmailSettings> emailsettings)
        {
            _smtpClient = new SmtpClient()
            {
                Host = emailsettings.Value.SmtpServer,
                Port = emailsettings.Value.SmtpPort,
                Credentials = new NetworkCredential(emailsettings.Value.Username, emailsettings.Value.Password),
                EnableSsl = true
            };
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailmessage = new MailMessage
            {
                From = new MailAddress(emailsettings.Value.Username),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailmessage.To.Add(email);
            await _smtpClient.SendMailAsync(mailmessage);
        }

    }
}
