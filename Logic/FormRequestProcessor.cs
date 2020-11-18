using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using stefanini_e_counter.Models;

namespace stefanini_e_counter.Logic
{
    public interface IFormRequestProcessor
    {
        Task<FormRequestResponse> ProcessForm(BaseFormRequest request);
    }

    public class FormRequestProcessor : IFormRequestProcessor
    {
        private readonly FormProcessingStrategy processingStrategy;
        private readonly IEmailFormProcessor emailProcessor;
        private readonly IDocumentProcessor documentProcessor;

        public FormRequestProcessor(IOptions<FormProcessingStrategy> processingStrategy, IEmailFormProcessor emailProcessor, IDocumentProcessor documentProcessor)
        {
            this.processingStrategy = processingStrategy.Value;
            this.emailProcessor = emailProcessor;
            this.documentProcessor = documentProcessor;
        }

        public Task<FormRequestResponse> ProcessForm(BaseFormRequest request)
        {
            switch (request)
            {
                case MedicalFormRequest medicalFormRequest:
                    return ProcessMedicalFormRequest(medicalFormRequest);
                case EmployeeFormRequest employeeFormRequest:
                    return ProcessEmployeeFormRequest(employeeFormRequest);
                case BankFormRequest bankFormRequest:
                    return ProcessBankFormRequest(bankFormRequest);
                default:
                    throw new ArgumentException($"Unkown type of request {request.GetType()}");
            }
        }

        private async Task<FormRequestResponse> ProcessMedicalFormRequest(MedicalFormRequest request)
        {
            switch (processingStrategy.MedicalForm)
            {
                case FormProcessingStrategyType.Email:
                    await emailProcessor.ProcessMedicalFormRequest(request);
                    return new FormRequestResponse() { ResponseType = FormRequestResponseType.EmailSent};
                case FormProcessingStrategyType.FillAndReturn:
                    var documentId = documentProcessor.CreateDocument(request.User, request.PurposeOfTheRequest);
                    return new FormRequestDocumentGeneratedResponse() { DocumentId = documentId};
                case FormProcessingStrategyType.PrefillAndEmail:
                    throw new InvalidOperationException($"Cannot process a medical form request using {processingStrategy.MedicalForm} strategy");
                default :
                    throw new ArgumentException($"Unkown type of strategy for Medical Form Request {processingStrategy.MedicalForm}");
            }
        }

        private async Task<FormRequestResponse> ProcessEmployeeFormRequest(EmployeeFormRequest request)
        {
            switch (processingStrategy.EmployeeForm)
            {
                case FormProcessingStrategyType.Email:
                    await emailProcessor.ProcessEmployeeFormRequest(request);
                    return new FormRequestResponse() { ResponseType = FormRequestResponseType.EmailSent};

                case FormProcessingStrategyType.FillAndReturn:
                case FormProcessingStrategyType.PrefillAndEmail:
                    throw new InvalidOperationException($"Cannot process a employee form request using {processingStrategy.EmployeeForm} strategy");
                default :
                    throw new ArgumentException($"Unkown type of strategy for Employee Form Request {processingStrategy.EmployeeForm}");
            }
        }

        private async Task<FormRequestResponse> ProcessBankFormRequest(BankFormRequest request)
        {
            switch (processingStrategy.BankForm)
            {
                case FormProcessingStrategyType.Email:
                    await emailProcessor.ProcessBankFormRequest(request);
                    return new FormRequestResponse() { ResponseType = FormRequestResponseType.EmailSent};

                case FormProcessingStrategyType.FillAndReturn:
                case FormProcessingStrategyType.PrefillAndEmail:
                    throw new InvalidOperationException($"Cannot process a bank form request using {processingStrategy.BankForm} strategy");
                default :
                    throw new ArgumentException($"Unkown type of strategy for Bank Form Request {processingStrategy.BankForm}");
            }
        }
    }
}