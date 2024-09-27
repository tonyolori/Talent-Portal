using Application.Common;
using Application.Common.Helpers;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using StackExchange.Redis;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.LearningAdmins.Commands;

public class LoginLearningAdminCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginAdminCommandHandler(SignInManager<LearningAdmin> signInManager, 
                                        UserManager<LearningAdmin> userManager,
                                        IHttpContextAccessor httpContextAccessor,
                                        IGenerateToken generateToken)
    : IRequestHandler<LoginLearningAdminCommand, Result>
{
    private readonly UserManager<LearningAdmin> _userManager = userManager;
    private readonly SignInManager<LearningAdmin> _signInManager = signInManager;
    private readonly IGenerateToken _generateToken = generateToken;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


    public async Task<Result> Handle(LoginLearningAdminCommand request, CancellationToken cancellationToken)
    {
        LearningAdmin? admin = await _userManager.FindByEmailAsync(request.Email);

        if (admin == null)
        {
            return Result.Failure<LoginLearningAdminCommand>("Admin Not Found.");
        }
        

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(admin, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {

                return Result.Failure<LoginLearningAdminCommand>("The login details provided are invalid. Kindly click on the forgot password to reset your password.");
            }
            return Result.Failure<LoginLearningAdminCommand>("Invalid Email or Password");
        }

        var tokens = _generateToken.GenerateTokens(admin.Id, admin.Email!, admin.RoleDesc);
        
        CookieHelper.SetTokensInCookies(_httpContextAccessor, tokens.AccessToken, tokens.RefreshToken);

        var tokenResponse = new TokenResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };

        return Result.Success("Successfully logged in");
    }
    
}
