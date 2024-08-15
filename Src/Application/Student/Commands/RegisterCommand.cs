//using Application.Common.DTOs;
//using Application.Common.Models;
//using Application.Interfaces;
//using Domain.Entities;
//using Domain.Enum;
//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace Application.Users.Commands;

//public class RegisterCommand : IRequest<Result>
//{
//    public StudentDto Student { get; set; }
//}

//public class AddUserCommandHandler(
//            IEmailService emailSender,
//            UserManager<Student> userManager,
//            RoleManager<IdentityRole> roleManager,
//            ISecretHasherService secretHasher)
// : IRequestHandler<RegisterCommand, Result>
//{
//    private readonly IEmailService _emailSender = emailSender;
//    private readonly UserManager<Student> _userManager = userManager;
//    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
//    private readonly ISecretHasherService _secretHasher = secretHasher;

//    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
//    {
//        //await request.ValidateAsync(new AddUserCommandValidator(), cancellationToken);

//        User? userExists = await _userManager.FindByEmailAsync(request.Student.Email);

//        if (userExists != null)
//            return Result.Failure(request, "User Already Exists");

//        User user = new()
//        {
//            AccountNumber = GenerateRandomAccountNumber(),
//            Balance = 0,
//            Email = request.Student.Email,
//            SecurityStamp = Guid.NewGuid().ToString(),
//            FirstName = request.Student.FirstName,
//            UserName = request.Student.FirstName + request.Student.LastName,
//            LastName = request.Student.LastName,
//            Pin = _secretHasher.Hash(request.Student.Pin.ToString()),
//            Status = Status.Active,
//            StatusDesc = Status.Active.ToString(),
//            CreatedDate = DateTime.Now,
//            LastModifiedDate = DateTime.Now,
//        };

//        IdentityResult result = await _userManager.CreateAsync(user, request.Student.Password);

//        if (!result.Succeeded)
//        {
//            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
//            return Result.Failure(request, "\"User creation failed!\n" + errors);
//        }


//        if (!await _roleManager.RoleExistsAsync(request.UserRole.ToString()))
//            await _roleManager.CreateAsync(new IdentityRole(request.UserRole.ToString()));

//        if (await _roleManager.RoleExistsAsync(request.UserRole.ToString()))
//        {
//            await _userManager.AddToRoleAsync(user, request.UserRole.ToString());
//        }


//        await _emailSender.SendEmailAsync(request.Student.Email, request.Student.FirstName);
//        return Result.Success(request,"User created successfully!");

//    }

//    private static long GenerateRandomAccountNumber()
//    {
//        Random random = new();
//        long accountId = 0;


//        for (int i = 0; i < 10; i++)
//        {
//            accountId = accountId * 10 + random.Next(1, 10); // Generate a random digit (0-9) and add it to the account number
//        }

//        return accountId;
//    }

//    public async Task<long> GenerateUniqueAccount(CancellationToken cancellationToken)
//    {
//        long newId;
//        bool exists;

//        do
//        {
//            newId = GenerateRandomAccountNumber();
//            exists = false; //TODO: find by account number
//        } while (exists);

//        return await Task.FromResult(newId);
//    }

//}
