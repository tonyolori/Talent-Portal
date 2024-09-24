using System.Text.RegularExpressions;
using Application.Auth.Commands;
using FluentValidation;

namespace Application.LearningAdmins.Commands
{
    public class LeaningAdminPasswordValidator : AbstractValidator<ResetLearningAdminPasswordCommand>
    {
        public LeaningAdminPasswordValidator()
        {
            // Email validation
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // Password reset code validation
            RuleFor(user => user.PasswordResetCode)
                .NotEmpty().WithMessage("Password reset code is required.");

            // New password validation
            RuleFor(user => user.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .Must(BeValidPassword)
                .WithMessage("Password must be at least 8 characters long, including at least one number and one special character.");

            // Confirm password validation
            RuleFor(user => user.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(user => user.NewPassword).WithMessage("New password and confirm password do not match.");
        }

        private static bool BeValidPassword(string password)
        {
            string pattern = @"^(?=.*\d)(?=.*\W)[a-zA-Z\d\W]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}