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
        private readonly EmailSettings _emailsettings;

        public EmailSender(IOptions<EmailSettings> emailsettings)
        {
            _emailsettings = emailsettings.Value;
            _smtpClient = new SmtpClient()
            {
                Host = _emailsettings.SmtpServer,
                Port = _emailsettings.SmtpPort,
                Credentials = new NetworkCredential(_emailsettings.Username,_emailsettings.Password),
                EnableSsl = true,
            };
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_emailsettings.Username);
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                // Ensure _smtpClient is properly configured
                await _smtpClient.SendMailAsync(mailMessage);

            }
        }


    }
}
