using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using stefanini_e_counter.Authentication;
using stefanini_e_counter.Logic;

namespace stefanini_e_counter
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            
            services.AddOptions();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.Configure<ECounterAuthentication>(Configuration.GetSection("authentication"));
            services.Configure<FormProcessingStrategy>(Configuration.GetSection("formProcessingStrategy"));
            services.Configure<EmailFormProcessorConfiguration>(Configuration.GetSection("emailFormMapping"));
            
            SmtpSettings smtpSettings = new SmtpSettings()
            {
                Server = Environment.GetEnvironmentVariable("MAILGUN_SMTP_SERVER"),
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("MAILGUN_SMTP_PORT")),
                UserName = Environment.GetEnvironmentVariable("MAILGUN_SMTP_LOGIN"),
                Password = Environment.GetEnvironmentVariable("MAILGUN_SMTP_PASSWORD"),
                SenderName = "Sophie-E-Counter",
                SenderEmail = "noreply@stefanini-e-counter.com"
            };
            Console.WriteLine(smtpSettings);
            services.AddSingleton<SmtpSettings>((serviceProvider) => smtpSettings);

            services.AddAuthentication("EzPzTokenAuth").AddScheme<AuthenticationSchemeOptions, EzPzAuthenticationHandler>("EzPzTokenAuth", null, null);
            services.AddAuthorization();
            services.AddSingleton<IEmailFormProcessor, EmailFormProcessor>();
            services.AddSingleton<IFormRequestProcessor, FormRequestProcessor>();
            services.AddSingleton<IDocumentProcessor, DocumentProcessor>();
            // Behind settings
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
