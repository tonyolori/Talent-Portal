
using System.Text.RegularExpressions;
using Domain.Enum;
using FluentValidation;

namespace Application.Students.Commands
{
    public class StudentCreateValidator : AbstractValidator<RegisterStudentCommand>
    {
        public StudentCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(50).WithMessage("Last Name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.");
           

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Must(BeValidPassword)
                .WithMessage("Password must be at least 8 characters long, including at least one number and one special character");

            RuleFor(user => user.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => Enum.TryParse<UserRoles>(role, true, out _))
                .WithMessage("Invalid role specified. Allowed roles are Admin, Student, and Instructor");

            RuleFor(x => x.Programme)
                .IsInEnum().WithMessage("Invalid Programme.");
        }

        private static bool BeValidPassword(string password)
        {
            string pattern = @"^(?=.*\d)(?=.*\W)[a-zA-Z\d\W]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }

    
}