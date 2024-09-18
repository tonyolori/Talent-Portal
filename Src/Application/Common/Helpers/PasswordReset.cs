using Application.Common.Helpers;
using Application.Interfaces;
using StackExchange.Redis;
using System.Threading.Tasks;

public static class PasswordReset
{
    public static async Task<string> SendPasswordResetVerificationCodeAsync(
        IConnectionMultiplexer redis,
        IEmailService emailService,
        string email)
    {
        // Get the Redis database instance
        var redisDb = redis.GetDatabase();

        // Generate the verification code
        string verificationCode = GenerateCode.GenerateRegistrationCode();

        // Store the reset code in Redis with an expiration of 2 hours
        await redisDb.StringSetAsync($"PasswordResetCode:{email}", verificationCode, TimeSpan.FromHours(2));

        // Send the verification code to the user's email
        await emailService.SendEmailAsync(email, "Password Reset Verification Code",
            $"Your verification code is {verificationCode}");

        // Return the generated verification code
        return verificationCode;
    }
}