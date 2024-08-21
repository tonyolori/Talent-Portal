using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands;

public class ForgotPasswordCommand : IRequest<Result>
{
    public string Email { get; set; }
}

public class ForgotPasswordCommandHandler(UserManager<Student> userManager, IEmailService emailService) : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly UserManager<Student> _userManager = userManager;
    private readonly IEmailService _emailService = emailService;

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        
        Student? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure("Email does not exist.");
        }

        string verificationCode = GenerateVerificationCode();

        user.PasswordResetCode = verificationCode;
        await _userManager.UpdateAsync(user);

        //Send the verification code to the user's email
        await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code",
            $"Your verification code is {verificationCode}");

        return Result.Success<ForgotPasswordCommand>("Email verification code sent!", verificationCode);
    }

    private string GenerateVerificationCode()
    {
        Random random = new ();
        return random.Next(100000, 999999).ToString(); // Generates a 6-digit random number
    }
}
