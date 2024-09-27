using Application.Admins.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using Application.Common.Models;
using Application.Extensions;
using Application.LearningAdmins.Commands;
using Domain.Entities;

namespace Application.Instructors.Commands
{
    public class ResetInstructorPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string PasswordResetCode { get; set; }
        public string NewPassword { get; set; }
        
        public string ConfirmPassword { get; set; }
    }

    public class ResetInstructorPasswordCommandHandler : IRequestHandler<ResetInstructorPasswordCommand, Result>
    {
        private readonly UserManager<LearningAdmin> _userManager;
        private readonly IDatabase _redisDb;

        public ResetInstructorPasswordCommandHandler(UserManager<LearningAdmin> userManager, IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Result> Handle(ResetInstructorPasswordCommand request, CancellationToken cancellationToken)
        {
            LearningAdmin? admin = await _userManager.FindByEmailAsync(request.Email);
            if (admin == null)
            {
                return Result.Failure<ResetInstructorPasswordCommand>("Invalid email.");
            }

            string storedResetCode = await _redisDb.StringGetAsync($"PasswordResetCode:{request.Email}");
            if (string.IsNullOrEmpty(storedResetCode) || storedResetCode != request.PasswordResetCode)
            {
                return Result.Failure<ResetInstructorPasswordCommand>("Invalid or expired password reset code.");
            }

            await request.ValidateAsync(new InstructorPasswordValidator(), cancellationToken);
            admin.PasswordHash = _userManager.PasswordHasher.HashPassword(admin, request.NewPassword);

            IdentityResult result = await _userManager.UpdateAsync(admin);
            if (!result.Succeeded)
            {
                return Result.Failure("Failed to reset the password.");
            }

            // Remove the reset code from Redis after successful password reset
            await _redisDb.KeyDeleteAsync($"PasswordResetCode:{request.Email}");

            return Result.Success("Password reset successfully.");
        }
    }
}
