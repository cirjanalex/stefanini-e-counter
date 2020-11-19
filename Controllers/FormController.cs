using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using stefanini_e_counter.Logic;
using stefanini_e_counter.Models;

namespace stefanini_e_counter.Controllers
{
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly IFormRequestProcessor formProcessor;

        public FormController(IFormRequestProcessor formProcessor)
        {
            this.formProcessor = formProcessor;
        }
        [HttpPost]
        [Route("medical")]
        public async Task<FormRequestResponse> RequestMedicalForm([FromBody] MedicalFormRequest medicalFormRequest)
        {
            return await ProcessForm(medicalFormRequest);
        }

        [HttpPost]
        [Route("employee")]
        public async Task<FormRequestResponse> RequestEmployeeForm([FromBody] EmployeeFormRequest employeeFormRequest)
        {
            return await ProcessForm(employeeFormRequest);
        }

        [HttpPost]
        [Route("bank")]        
        public async Task<FormRequestResponse> RequestBankForm([FromQuery] string user)
        {
            IFormFile bankForm = Request.Form.Files.FirstOrDefault();
            if(bankForm == null)
            {
                Response.StatusCode = 400;
                return FormRequestResponse.ErrorResponse;
            }

            return await ProcessForm(new BankFormRequest() { 
                File = bankForm,
                User = user
            });
        }

        [HttpPost]
        [Route("bankstandard")]        
        public async Task<FormRequestResponse> RequestStandardBankForm([FromBody] StandardBankFormRequest request)
        {
            return await ProcessForm(request);
        }
        
        [HttpPost]
        [Route("other")]        
        public async Task<FormRequestResponse> OtherForm([FromBody] FormRequestWithPurpose request)
        {
            return await ProcessForm(request);
        }

        private async Task<FormRequestResponse> ProcessForm(BaseFormRequest formRequest)
        {
            try
            {
                return await formProcessor.ProcessForm(formRequest);
            }
            catch
            {
                return FormRequestResponse.ErrorResponse;
            }
        }
    }
}