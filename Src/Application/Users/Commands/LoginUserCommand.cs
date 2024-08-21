using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Enum;
using Domain.Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands;

public class LoginUserCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class StudentLoginCommandHandler(SignInManager<Student> signInManager, UserManager<Student> userManager,
    IGenerateToken generateToken) : IRequestHandler<LoginUserCommand, Result>
{
    private readonly UserManager<Student> _userManager = userManager;
    private readonly SignInManager<Student> _signInManager = signInManager;
    private readonly IGenerateToken _generateToken = generateToken;

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {

        Student? user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.Failure<LoginUserCommand>("User Not Found.");
        }

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (user.UserStatus != Status.Active)
        {
            return Result.Failure<LoginUserCommand>($"User {request.Email} account is not active.");
        }


        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {
                user.UserStatus = Status.Suspended;
                user.UserStatusDes = Status.Suspended.ToString();
                await _userManager.UpdateAsync(user);

                return Result.Failure<LoginUserCommand>($"User {request.Email} account locked: Unsuccessful 3 login attempts.");
            }
            return Result.Failure<LoginUserCommand>("Invalid Email or Password");
        }

        string token = _generateToken.GenerateToken(user.Email, user.RoleDesc);
        return Result.Success(token);
    }
}