using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SiteTGS.Models;
using System.Threading.Tasks;

namespace SiteTGS.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest request)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
            string emails = request.ToEmail;
            if (emails.Contains(";"))
            {
                var arrEmails = emails.Split(";");
                for (int i = 0; i < arrEmails.Length; i++)
                {
                    email.To.Add(MailboxAddress.Parse(arrEmails[i]));
                }
            }
            else
            {
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
            }
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
