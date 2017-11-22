using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Text;
using Auctus.Util.NotShared;

namespace Auctus.Util
{
    public static class Email
    {
        public static void Send(IEnumerable<string> to, string subject, string body, bool bodyIsHtml = true, string from = "noreply@auctus.org", 
            IEnumerable<string> cc = null, IEnumerable<string> bcc = null, IEnumerable<Attachment> attachment = null)
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
                        foreach (Attachment a in attachment)
                            mailMessage.Attachments.Add(a);
                    }

                    client.Send(mailMessage);
                }
            }
        }
    }
}
