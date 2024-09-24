using Application.Common.Helpers;
using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;


namespace Application.LearningAdmins.Commands
{
    public class ForgotLearningAdminPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }
 
    public class ForgotAdminPasswordCommandHandler : IRequestHandler<ForgotLearningAdminPasswordCommand, Result>
    {
        private readonly UserManager<LearningAdmin> _userManager;
        private readonly IEmailService _emailService;
        private readonly IDatabase _redisDb;

        public ForgotAdminPasswordCommandHandler(UserManager<LearningAdmin> userManager, IEmailService emailService, IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _emailService = emailService;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Result> Handle(ForgotLearningAdminPasswordCommand request, CancellationToken cancellationToken)
        {
            LearningAdmin? admin = await _userManager.FindByEmailAsync(request.Email);
            if (admin == null)
            {
                return Result.Failure("Admin email does not exist.");
            }

            // Use the PasswordReset helper to send the password reset verification code
            string verificationCode = await PasswordReset.SendPasswordResetVerificationCodeAsync(_redisDb.Multiplexer, _emailService, request.Email);


            return Result.Success<ForgotLearningAdminPasswordCommand>("Email verification code sent!", verificationCode);
        }
        
    }
}