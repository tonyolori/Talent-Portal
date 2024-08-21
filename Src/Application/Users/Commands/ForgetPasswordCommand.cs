using Application.Common.Helpers;
using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;


namespace Application.Users.Commands
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly UserManager<Student> _userManager;
        private readonly IEmailService _emailService;
        private readonly IDatabase _redisDb;

        public ForgotPasswordCommandHandler(UserManager<Student> userManager, IEmailService emailService, IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _emailService = emailService;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            Student? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure("Email does not exist.");
            }

            string verificationCode = GenerateCode.GenerateRegistrationCode();;

            // Store the reset code in Redis with an expiration of 2 hours
            await _redisDb.StringSetAsync($"PasswordResetCode:{request.Email}", verificationCode, TimeSpan.FromHours(2));

            // Send the verification code to the user's email
            await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code",
                $"Your verification code is {verificationCode}");

            return Result.Success<ForgotPasswordCommand>("Email verification code sent!", verificationCode);
        }
        
    }
}
