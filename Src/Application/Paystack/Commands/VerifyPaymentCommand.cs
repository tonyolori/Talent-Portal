using System.Net.Http.Headers;
using System.Net.Http.Json;
using Domain.Entities;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Paystack.Commands
{
    public class VerifyPaymentCommand : IRequest<Result>
    {
        
        public string Email { get; set; }
        public string TransactionReference { get; set; }
    }

    public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;

        public VerifyPaymentCommandHandler(UserManager<User> userManager,HttpClient httpClient, IConfiguration configuration, IApplicationDbContext context)
        {
            _userManager = userManager;
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        public async Task<Result> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        {
            
            User? student  = await _userManager.FindByEmailAsync(request.Email);
            if (student == null)
            {
                return Result.Failure("The email does not belong to a registered student.");
            }
            // Get Paystack secret key from configuration
            var paystackSecretKey = _configuration["Paystack:SecretKey"];
            if (string.IsNullOrEmpty(paystackSecretKey))
            {
                return Result.Failure("Paystack secret key is not configured.");
            }

            // Set authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

            // Make API call to Paystack to verify the transaction
            var response = await _httpClient.GetAsync($"https://api.paystack.co/transaction/verify/{request.TransactionReference}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure("Payment verification failed.");
            }

            // Deserialize the response
            var responseData = await response.Content.ReadFromJsonAsync<PaystackVerifyResponse>(cancellationToken: cancellationToken);
            if (responseData?.Data == null || string.IsNullOrEmpty(responseData.Data.Status))
            {
                return Result.Failure("Invalid response from Paystack.");
            }

            string paymentStatus = responseData.Data.Status;

            // Fetch the transaction from the database
            Transaction? transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionReference == request.TransactionReference, cancellationToken);

            if (transaction == null)
            {
                return Result.Failure("Transaction not found.");
            }

            // Update transaction status
            transaction.TransactionStatus = paymentStatus;
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            // Check payment status and return appropriate result
            if (paymentStatus == "success")
            {
                student.PaymentType = transaction.PaymentType;
                student.PaymentTypeDes = transaction.PaymentType.ToString(); 
                await _userManager.UpdateAsync(student);
                return Result.Success("Payment verified successfully.");
            }

            return Result.Failure($"Payment failed with status: {paymentStatus}");
        }
    }

    // Class to deserialize the Paystack verification response
    public class PaystackVerifyResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public PaystackVerifyData Data { get; set; }

        public class PaystackVerifyData
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("reference")]
            public string Reference { get; set; }

            [JsonPropertyName("amount")]
            public int Amount { get; set; } // Amount in kobo
        }
    }
}
