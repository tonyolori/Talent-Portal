using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Instructors.Commands;

public class LoginInstructorCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginAdminCommandHandler(SignInManager<Instructor> signInManager, 
    UserManager<Instructor> userManager,
    IGenerateToken generateToken)
    : IRequestHandler<LoginInstructorCommand, Result>
{
    private readonly UserManager<Instructor> _userManager = userManager;
    private readonly SignInManager<Instructor> _signInManager = signInManager;
    private readonly IGenerateToken _generateToken = generateToken;


    public async Task<Result> Handle(LoginInstructorCommand request, CancellationToken cancellationToken)
    {
        Instructor? instructor = await _userManager.FindByEmailAsync(request.Email);

        if (instructor == null)
        {
            return Result.Failure<LoginInstructorCommand>("Instructor Not Found.");
        }
        

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(instructor, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {

                return Result.Failure<LoginInstructorCommand>("The login details provided are invalid. Kindly click on the forgot password to reset your password.");
            }
            return Result.Failure<LoginInstructorCommand>("Invalid Email or Password");
        }

        var token = _generateToken.GenerateTokens(instructor.Id,instructor.Email, instructor.RoleDesc);
        return Result.Success(token);
    }
    
}