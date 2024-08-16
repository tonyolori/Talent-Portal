using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.Enum;

namespace Application.Users.Commands
{
    public class LoginUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result>
    {
        private readonly UserManager<BaseUser> _userManager;
        private readonly SignInManager<BaseUser> _signInManager;
        private readonly IGenerateToken _generateToken;

        public LoginUserCommandHandler(SignInManager<BaseUser> signInManager, UserManager<BaseUser> userManager,
            IGenerateToken generateToken)
        {
            _signInManager = signInManager;
            _generateToken = generateToken;
            _userManager = userManager;
        }

        public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            BaseUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<LoginUserCommand>("Invalid login attempt.");
            }
            
            if (user.UserStatus != Status.Active)
            {
                return Result.Failure<LoginUserCommand>($"User {request.Email} account is not active.");
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: true);
            
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    user.UserStatus = Status.Suspended;
                    user.UserStatusDes = Status.Suspended.ToString();
                    await _userManager.UpdateAsync(user);

                    return Result.Failure<LoginUserCommand>($"User {request.Email} account locked: Unsuccessful 3 login attempts.");
                }
                return Result.Failure<LoginUserCommand>("Invalid login attempt.");
            }

            string token = _generateToken.GenerateToken(user.Email, user.RoleDesc);
            return Result.Success(token);
        }
    }
}
