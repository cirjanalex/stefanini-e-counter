using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
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
    }

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
            await SendEmail($"Adeverinta formular bancar standard {message.User}","", emailFormMapping.BankForm);
        }

        public async Task ProcessEmployeeFormRequest(EmployeeFormRequest message)
        {
            await SendEmail($"Adeverinta salariat {message.User}",$"Scopul adeverintei: {message.PurposeOfTheRequest}", emailFormMapping.EmployeeForm);
        }

        public async Task ProcessMedicalFormRequest(MedicalFormRequest message)
        {
            await SendEmail($"Adeverinta medicala {message.User}",$"Scopul adeverintei: {message.PurposeOfTheRequest}", emailFormMapping.EmployeeForm);
        }

        private async Task SendEmail(string subject, string messageBody, string toEmailAdress)
        {
            Console.WriteLine($"Email -> Subject: {subject}, Body: {messageBody}, Adress: {toEmailAdress}");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
            message.To.Add(new MailboxAddress(toEmailAdress));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Text) 
            {
                Text = messageBody
            };

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