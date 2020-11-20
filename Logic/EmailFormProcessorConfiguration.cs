using stefanini_e_counter.Models;

namespace stefanini_e_counter.Logic
{
    public class EmailFormProcessorConfiguration
    {
        public string MedicalForm { get; set; }
        public string EmployeeForm { get; set; }
        public string BankForm { get; set; }
        public string OtherForm { get; set; }
        public string Feedback { get; set; }
    }
}