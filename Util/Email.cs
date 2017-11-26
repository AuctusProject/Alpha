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
        public static void Send(IEnumerable<string> to, string subject, string body, bool bodyIsHtml = true, string from = "noreply@auctus.org", 
            IEnumerable<string> cc = null, IEnumerable<string> bcc = null, IEnumerable<System.Net.Mail.Attachment> attachment = null)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException("Parameter subject must be filled.");

            if (to == null || to.Count() == 0)
                throw new ArgumentNullException("Parameter to must be filled.");

            using (SmtpClient client = new SmtpClient(Config.SMTP_SERVER))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Config.SMTP_USER, Config.SMTP_PASSWORD);
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(from);
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = bodyIsHtml;
                    mailMessage.Subject = subject;

                    foreach (string t in to)
                        mailMessage.To.Add(t);

                    if (cc != null)
                    {
                        foreach (string c in cc)
                            mailMessage.CC.Add(c);
                    }

                    if (bcc != null)
                    {
                        foreach (string b in bcc)
                            mailMessage.Bcc.Add(b);
                    }

                    if (attachment != null)
                    {
                        foreach (System.Net.Mail.Attachment a in attachment)
                            mailMessage.Attachments.Add(a);
                    }

                    client.Send(mailMessage);
                }
            }
        }

        public static async System.Threading.Tasks.Task SendGridEmailAsync()
        {
            var client = new SendGridClient(Config.SENDGRID_API_KEY);
            var from = new EmailAddress("daniel.vitorino@auctus.org", "Daniel Vitorino");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("dansilveira@gmail.com", "Daniel Silveira");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
