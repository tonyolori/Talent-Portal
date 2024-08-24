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
            IConfigurationSection emailConfig = configuration.GetSection("Mail");
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

        public async Task<Result> SendWelcomeEmailAsync(string toEmail, string firstName)
        {
            string subject = EmailTemplate.GetSubject(firstName, "Revent Learning");
            string body = EmailTemplate.GetBody(firstName, "Revent Learning");
            MailMessage mailMessage = new (_fromEmail, toEmail, subject, body);

            return await SendEmail(toEmail, mailMessage);
        }

        public async Task<Result> SendEmailAsync(string toEmail, string subject, string body)
        {
            MailMessage mailMessage = new(_fromEmail, toEmail, subject, body);

            return await SendEmail(toEmail, mailMessage);
        }

        private async Task<Result> SendEmail(string toEmail, MailMessage mailMessage)
        {
            _smtpClient.Host = _smtpServer;
            _smtpClient.Port = _port;
            _smtpClient.Credentials = new NetworkCredential(_fromEmail, _password);
            _smtpClient.EnableSsl = true;

            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
                return Result.Success($"Email successfully sent to {toEmail}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to send email to {toEmail}", ex);
            }
        }
    }
}