using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Enum;
using Infrastructure.Templates;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly IConfiguration _configuration;
        private readonly string _password;
        private readonly string _smtpServer;
        private readonly int _port;

        public EmailService(IConfiguration configuration)
        {
            IConfigurationSection emailConfig = configuration.GetSection("ConnectionStrings");
            _fromEmail = emailConfig["Email"];
            _password = emailConfig["Password"];
            _smtpServer = emailConfig["SmtpServer"];


            if (!int.TryParse(emailConfig["Port"], out _port))
            {
                throw new InvalidOperationException("Invalid port value in configuration");
            }

            _smtpClient = new SmtpClient(_smtpServer, _port)
            {
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true,
                UseDefaultCredentials = false
            };
        }

        public async Task<Result> SendEmailAsync(string toEmail, EmailType type, string firstName, string subject = "", string body = "")
        {
            _smtpClient.Host = _smtpServer;
            _smtpClient.Port = _port;
            _smtpClient.Credentials = new NetworkCredential(_fromEmail, _password);
            _smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            if (type == EmailType.WelcomeMessage)
            {
                string s = EmailTemplate.GetSubject(firstName, "Revent Learning");
                string b = EmailTemplate.GetBody(firstName, "Revent Learning");
                mailMessage = new MailMessage(_fromEmail, toEmail, s, b);

            }
            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
                return Result.Success($"Registration Email successfully sent to {toEmail}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to send email to {toEmail}", ex);
            }
        }
    }
}