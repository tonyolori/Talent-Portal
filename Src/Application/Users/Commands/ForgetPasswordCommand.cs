using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class ForgotPasswordCommand : IRequest<Result>
{
    public string Email { get; set; }
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly UserManager<Student> _userManager;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(UserManager<Student> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure("Email does not exist.");
        }

        var verificationCode = GenerateVerificationCode();

        // You can store this verification code in the database associated with the user for validation later
        user.PasswordResetCode = verificationCode;
        await _userManager.UpdateAsync(user);

        // Send the verification code to the user's email
        // await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code",
        //     $"Your verification code is {verificationCode}");

        return Result.Success<ForgotPasswordCommand>( "Email verification code sent!", verificationCode );
    }

    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // Generates a 6-digit random number
    }
}