using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using stefanini_e_counter.Logic;
using stefanini_e_counter.Models;

namespace stefanini_e_counter.Controllers
{
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly EmailFormProcessorConfiguration emailFormMapping;
        public FeedbackController(IEmailService emailService, IOptions<EmailFormProcessorConfiguration> emailFormMapping)
        {
            this.emailService = emailService;
            this.emailFormMapping = emailFormMapping.Value;
        }   

        [HttpPost]
        public void Post([FromBody] FeedbackModel feedback)
        {
            this.emailService.SendEmail("Feedback Sophie - E-Counter", $"The user {feedback.User} has left the following feedback:\r\n {feedback.Feedback}", emailFormMapping.Feedback);
        }
    }
}