namespace stefanini_e_counter.Models
{
    public class BaseFormRequest
    {
        public string User { get; init; }
    }

    public class FormRequestWithPurpose : BaseFormRequest
    {
        public string PurposeOfTheRequest { get; init; }
    }

    public class MedicalFormRequest : FormRequestWithPurpose { }
    public class EmployeeFormRequest : FormRequestWithPurpose { }
    public class BankFormRequest : BaseFormRequest 
    {
        // pe undeva paci trebuie sa fie formularul        
        public string File {get; init; }
    }
}