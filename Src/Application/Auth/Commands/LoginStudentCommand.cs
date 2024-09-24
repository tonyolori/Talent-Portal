using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands;

public class LoginStudentCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class StudentLoginCommandHandler(SignInManager<Student> signInManager, 
                                        UserManager<Student> userManager,
                                        IGenerateToken generateToken, 
                                        IEmailService emailService, 
                                        IConnectionMultiplexer redis) 
    : IRequestHandler<LoginStudentCommand, Result>
{
    private readonly UserManager<Student> _userManager = userManager;
    private readonly SignInManager<Student> _signInManager = signInManager;
    private readonly IGenerateToken _generateToken = generateToken;
    private readonly IEmailService _emailService = emailService;
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task<Result> Handle(LoginStudentCommand request, CancellationToken cancellationToken)
    {
        Student? user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Failure<LoginStudentCommand>("User Not Found.");
        }

        if (!user.IsVerified)
        {
            // Generate and send a new confirmation code
            string confirmationCode = GenerateCode.GenerateRegistrationCode();

            // Store the confirmation code in Redis with an expiration of 2 hours
            await _redisDb.StringSetAsync($"ConfirmationCode:{request.Email}", confirmationCode, TimeSpan.FromHours(2));

            // Send the confirmation code to the user's email
            await _emailService.SendEmailAsync(user.Email, "Account Confirmation Code",
                $"Your confirmation code is {confirmationCode}");

            return Result.Failure<LoginStudentCommand>($"User {request.Email} account is not verified. A new confirmation code has been sent.");
        }

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {
                user.UserStatus = Status.Suspended;
                user.UserStatusDes = Status.Suspended.ToString();
                await _userManager.UpdateAsync(user);

                return Result.Failure<LoginStudentCommand>($"User {request.Email} account locked: Unsuccessful 3 login attempts.");
            }
            return Result.Failure<LoginStudentCommand>("Invalid Email or Password");
        }

        string token = _generateToken.GenerateToken(user.Id,user.Email, user.RoleDesc);
        return Result.Success(token);
    }
    
}
