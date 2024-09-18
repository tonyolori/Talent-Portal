using Application.Common.Helpers;
using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;


namespace Application.Auth.Commands
{
    public class ForgotAdminPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class ForgotAdminPasswordCommandHandler : IRequestHandler<ForgotAdminPasswordCommand, Result>
    {
        private readonly UserManager<Admin> _userManager;
        private readonly IEmailService _emailService;
        private readonly IDatabase _redisDb;

        public ForgotAdminPasswordCommandHandler(UserManager<Admin> userManager, IEmailService emailService, IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _emailService = emailService;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Result> Handle(ForgotAdminPasswordCommand request, CancellationToken cancellationToken)
        {
            Admin? admin = await _userManager.FindByEmailAsync(request.Email);
            if (admin == null)
            {
                return Result.Failure("Admin email does not exist.");
            }

            // Use the PasswordReset helper to send the password reset verification code
            string verificationCode = await PasswordReset.SendPasswordResetVerificationCodeAsync(_redisDb.Multiplexer, _emailService, request.Email);


            return Result.Success<ForgotAdminPasswordCommand>("Email verification code sent!", verificationCode);
        }
        
    }
}