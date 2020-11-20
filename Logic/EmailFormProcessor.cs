using System.Threading.Tasks;
using Microsoft.Extensions.Options;
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
        private readonly IEmailService emailService;
        
        public EmailFormProcessor(IOptions<EmailFormProcessorConfiguration> emailFormMapping, IEmailService emailService)
        {
            this.emailFormMapping = emailFormMapping.Value;
            this.emailService = emailService;
        }

        public async Task ProcessBankFormRequest(BankFormRequest message)
        {
            await emailService.SendEmail($"Adeverinta formular bancar {message.User}", "Va rog sa completati formularul atasat.", emailFormMapping.BankForm, message.File);
        }

        public async Task ProcessStandardBankFormRequest(StandardBankFormRequest message)
        {
            await emailService.SendEmail($"Adeverinta formular bancar standard {message.User}", "Va rog sa imi trimiteti un formular pentru banca.", emailFormMapping.BankForm);
        }

        public async Task ProcessEmployeeFormRequest(EmployeeFormRequest message)
        {
            await emailService.SendEmail($"Adeverinta salariat {message.User}", $"Scopul adeverintei: {message.PurposeOfTheRequest}", emailFormMapping.EmployeeForm);
        }

        public async Task ProcessMedicalFormRequest(MedicalFormRequest message)
        {
            await emailService.SendEmail($"Adeverinta medicala {message.User}", $"Scopul adeverintei: {message.PurposeOfTheRequest}", emailFormMapping.MedicalForm);
        }

        public async Task ProcessOtherFormRequest(FormRequestWithPurpose message)
        {
            await emailService.SendEmail($"Cerere custom pentru {message.User}", $"{message.User} a facut o cerere cu urmatoarea descriere: {message.PurposeOfTheRequest}", emailFormMapping.OtherForm);
        }
    }
}