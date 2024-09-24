using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;


namespace Application.Instructors.Commands
{
    public class ForgotInstructorPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }
 
    public class ForgotInstructorPasswordCommandHandler : IRequestHandler<ForgotInstructorPasswordCommand, Result>
    {
        private readonly UserManager<Instructor> _userManager;
        private readonly IEmailService _emailService;
        private readonly IDatabase _redisDb;

        public ForgotInstructorPasswordCommandHandler(UserManager<Instructor> userManager, IEmailService emailService, IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _emailService = emailService;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Result> Handle(ForgotInstructorPasswordCommand request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _userManager.FindByEmailAsync(request.Email);
            if (instructor == null)
            {
                return Result.Failure("Instructor email does not exist.");
            }

            // Use the PasswordReset helper to send the password reset verification code
            string verificationCode = await PasswordReset.SendPasswordResetVerificationCodeAsync(_redisDb.Multiplexer, _emailService, request.Email);


            return Result.Success<ForgotInstructorPasswordCommand>("Email verification code sent!", verificationCode);
        }
        
    }
}