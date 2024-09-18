using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands;

public class LoginAdminCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginAdminCommandHandler(SignInManager<Admin> signInManager, 
                                        UserManager<Admin> userManager,
                                        IGenerateToken generateToken)
    : IRequestHandler<LoginAdminCommand, Result>
{
    private readonly UserManager<Admin> _userManager = userManager;
    private readonly SignInManager<Admin> _signInManager = signInManager;
    private readonly IGenerateToken _generateToken = generateToken;


    public async Task<Result> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        Admin? admin = await _userManager.FindByEmailAsync(request.Email);

        if (admin == null)
        {
            return Result.Failure<LoginUserCommand>("Admin Not Found.");
        }
        

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(admin, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {

                return Result.Failure<LoginUserCommand>("The login details provided are invalid. Kindly click on the forgot password to reset your password.");
            }
            return Result.Failure<LoginUserCommand>("Invalid Email or Password");
        }

        string token = _generateToken.GenerateToken(admin.Id,admin.Email, admin.RoleDesc);
        return Result.Success(token);
    }
    
}
