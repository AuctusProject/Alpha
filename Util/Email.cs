using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Text;
using Auctus.Util.NotShared;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Auctus.Util
{
    public static class Email
    {
        public static async System.Threading.Tasks.Task SendAsync(IEnumerable<string> to, string subject, string body, bool bodyIsHtml = true, string from = "noreply@auctus.org", 
            IEnumerable<string> cc = null, IEnumerable<string> bcc = null, IEnumerable<SendGrid.Helpers.Mail.Attachment> attachment = null)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException("Parameter subject must be filled.");

            if (to == null || to.Count() == 0)
                throw new ArgumentNullException("Parameter to must be filled.");

            List<EmailAddress> toList = new List<EmailAddress>();

            foreach (string t in to)
                toList.Add(new EmailAddress(t));          

            var mailMessage = MailHelper.CreateSingleEmailToMultipleRecipients(new EmailAddress(from, "Auctus Mail Service"), toList, subject, bodyIsHtml ? null : body, bodyIsHtml ? body : null);

            if (attachment != null)
            {
                foreach (SendGrid.Helpers.Mail.Attachment a in attachment)
                    mailMessage.Attachments.Add(a);
            }

            if (cc != null)
            {
                foreach (string c in cc)
                    mailMessage.AddCc(c);
            }

            if (bcc != null)
            {
                foreach (string b in bcc)
                    mailMessage.AddBcc(b);
            }

            SendGridClient client = new SendGridClient(Config.SENDGRID_API_KEY);

            var response = await client.SendEmailAsync(mailMessage);
        }
    }
}
