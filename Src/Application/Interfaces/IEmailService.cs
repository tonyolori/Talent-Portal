using Application.Common.Models;
using Domain.Common.Enum;

namespace Application.Interfaces;
public interface IEmailService
{
    Task<Result> SendWelcomeEmailAsync(string toEmail, string firstName);
    Task<Result> SendEmailAsync(string toEmail, string subject, string body);  
}
