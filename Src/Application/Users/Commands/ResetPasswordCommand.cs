using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Extensions;
using Application.Users;
using Domain.Entities;

public class ResetPasswordCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string PasswordResetCode { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<Student> _userManager;


    public ResetPasswordCommandHandler(UserManager<Student> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || user.PasswordResetCode != request.PasswordResetCode)
        {
            return Result.Failure<ResetPasswordCommand>("Invalid email or password reset code.");
        }
        
        await request.ValidateAsync(new PasswordValidator() , cancellationToken);

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
        user.PasswordResetCode = ""; 

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Failure("Failed to reset the password" );
        }

        return Result.Success("Password reset successfully");
    }
}
