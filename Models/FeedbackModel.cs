using Microsoft.AspNetCore.Http;

namespace stefanini_e_counter.Models
{
    public class FeedbackModel
    {
        public string User { get; init; }
        public string Feedback { get; init; }
    }
}