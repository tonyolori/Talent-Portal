using Application.Common.Models;
using Domain.Common.Enum;

namespace Application.Interfaces;
public interface IEmailService
{
    Task<Result> SendEmailAsync(string toEmail, EmailType type, string firstName, string subject="", string body="");
}
