using stefanini_e_counter.Models;

namespace stefanini_e_counter.Logic
{
    public class FormProcessingStrategy
    {
        public FormProcessingStrategyType MedicalForm { get; set; }
        public FormProcessingStrategyType EmployeeForm { get; set; }
        public FormProcessingStrategyType BankForm { get; set; }
        public FormProcessingStrategyType StandardBankForm { get; set; }
    }

    public enum FormProcessingStrategyType
    {
        Email,
        PrefillAndEmail,
        FillAndReturn
    }
}