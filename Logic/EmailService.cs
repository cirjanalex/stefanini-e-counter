using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using stefanini_e_counter.Models;

namespace stefanini_e_counter.Logic
{
    public interface IEmailService
    {
        Task SendEmail(string subject, string messageBody, string toEmailAdress, params IFormFile[]  attachments);
    }

    // All the strings in this class should be localized
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings smtpSettings;

        public EmailService(SmtpSettings smtpSettings)
        {
            this.smtpSettings = smtpSettings;
        }

        public async Task SendEmail(string subject, string messageBody, string toEmailAdress, params IFormFile[]  attachments)
        {
            Console.WriteLine($"Email -> Subject: {subject}, Body: {messageBody}, Adress: {toEmailAdress}");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
            message.To.Add(new MailboxAddress(toEmailAdress));
            message.Subject = subject;
            
            var builder = new BodyBuilder ();
            builder.TextBody = messageBody;
            foreach(var attachment in attachments)
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    attachment.OpenReadStream().CopyTo(stream);
                    var bytes = stream.ToArray();
                    builder.Attachments.Add(attachment.FileName, bytes);
                }
            }
            
            message.Body = builder.ToMessageBody();
            using(var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s,c,h,e) => true;

                await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port);
                await client.AuthenticateAsync(smtpSettings.UserName, smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}