namespace stefanini_e_counter.Models
{
    public class FormRequestResponse
    {
        public virtual FormRequestResponseType ResponseType { get; init; }

        public static FormRequestResponse ErrorResponse => new FormRequestResponse() { ResponseType = FormRequestResponseType.Error };
    }

    public class FormRequestDocumentGeneratedResponse : FormRequestResponse
    {
        public string DocumentId { get; init; }

        public override FormRequestResponseType ResponseType { get => FormRequestResponseType.DocumentGenerated; }
    }

    public enum FormRequestResponseType
    {
        Error,
        EmailSent,
        DocumentGenerated,
    }
}