using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Enum;
using Domain.Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands
{
    public class LoginUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler(SignInManager<BaseUser> signInManager, UserManager<BaseUser> userManager,
        IGenerateToken generateToken) : IRequestHandler<LoginUserCommand, Result>
    {
        private readonly UserManager<BaseUser> _userManager = userManager;
        private readonly SignInManager<BaseUser> _signInManager = signInManager;
        private readonly IGenerateToken _generateToken = generateToken;

        public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {

            //var user = await _userManager.FindByEmailAsync(request.Email);

            //if (user == null)
            //{
            //    // ...
            //}

            //Student? user = await _userManager.FindByEmailAsync(request.Email);
            //if (user == null)
            //{
            //    return Result.Failure<LoginUserCommand>("Invalid login attempt.");
            //}
            //// Cast to specific user type if needed
            //var studentUser = user as Student;
            //var teacherUser = user as Teacher;

            //// Use the correct SignInManager based on user type
            //var signInResult = await _signInManager.PasswordSignInAsync(
            //    studentUser ?? teacherUser, // Use the correct user type
            //    request.Password,
            //    isPersistent: false,
            //    lockoutOnFailure: true);


            //if (user.UserStatus != Status.Active)
            //{
            //    return Result.Failure<LoginUserCommand>($"User {request.Email} account is not active.");
            //}

            //SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: true);

            //if (!result.Succeeded)
            //{
            //    if (result.IsLockedOut)
            //    {
            //        user.UserStatus = Status.Suspended;
            //        user.UserStatusDes = Status.Suspended.ToString();
            //        await _userManager.UpdateAsync(user);

            //        return Result.Failure<LoginUserCommand>($"User {request.Email} account locked: Unsuccessful 3 login attempts.");
            //    }
            //    return Result.Failure<LoginUserCommand>("Invalid login attempt.");
            //}

            //string token = _generateToken.GenerateToken(user.Email, user.RoleDesc);
            //return Result.Success(token);
            return Result.Success(":asdfga");
        }
    }
}
