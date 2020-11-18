using stefanini_e_counter.Models;

namespace stefanini_e_counter.Logic
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"EmailSettings: {Server}:{Port}, Username: {UserName}, Password: {Password}";
        }
    }
}