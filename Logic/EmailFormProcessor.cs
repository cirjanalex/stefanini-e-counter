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
    public interface IEmailFormProcessor
    {
        Task ProcessMedicalFormRequest(MedicalFormRequest message);
        Task ProcessEmployeeFormRequest(EmployeeFormRequest message);
        Task ProcessBankFormRequest(BankFormRequest message);
        Task ProcessStandardBankFormRequest(StandardBankFormRequest message);
        Task ProcessOtherFormRequest(FormRequestWithPurpose message);
    }

    // All the strings in this class should be localized
    public class EmailFormProcessor : IEmailFormProcessor
    {
        private readonly EmailFormProcessorConfiguration emailFormMapping;
        private readonly SmtpSettings smtpSettings;
        public EmailFormProcessor(IOptions<EmailFormProcessorConfiguration> emailFormMapping, SmtpSettings smtpSettings)
        {
            this.emailFormMapping = emailFormMapping.Value;
            this.smtpSettings = smtpSettings;
        }

        public async Task ProcessBankFormRequest(BankFormRequest message)
        {
            await SendEmail($"Adeverinta formular bancar {message.User}","Va rog sa completati formularul atasat.", emailFormMapping.BankForm, message.File);
        }

        public async Task ProcessStandardBankFormRequest(StandardBankFormRequest message)
        {
            await SendEmail($"Adeverinta formular bancar standard {message.User}","Va rog sa imi trimiteti un formular pentru banca.", emailFormMapping.BankForm);
        }

        public async Task ProcessEmployeeFormRequest(EmployeeFormRequest message)
        {
            await SendEmail($"Adeverinta salariat {message.User}",$"Scopul adeverintei: {message.PurposeOfTheRequest}", emailFormMapping.EmployeeForm);
        }

        public async Task ProcessMedicalFormRequest(MedicalFormRequest message)
        {
            await SendEmail($"Adeverinta medicala {message.User}",$"Scopul adeverintei: {message.PurposeOfTheRequest}", emailFormMapping.MedicalForm);
        }

        public async Task ProcessOtherFormRequest(FormRequestWithPurpose message)
        {
            await SendEmail($"Cerere custom pentru {message.User}",$"{message.User} a facut o cerere cu urmatoarea descriere: {message.PurposeOfTheRequest}", emailFormMapping.OtherForm);
        }

        private async Task SendEmail(string subject, string messageBody, string toEmailAdress, params IFormFile[]  attachments)
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