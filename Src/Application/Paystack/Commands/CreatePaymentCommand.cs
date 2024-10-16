using System.Net.Http.Headers;
using System.Net.Http.Json;
using Domain.Entities;
using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using System.Text.Json.Serialization;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Paystack.Commands
{
    public class CreatePaymentCommand : IRequest<Result>
    {
        
        [JsonIgnore]
        public string? Email { get; set; }
        public decimal Amount { get; set; }
        public string PreferredProgramme { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public EducationalLevel EducationalLevel { get; set; }
        public PaymentType PaymentType { get; set; }
    }

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreatePaymentCommandHandler(UserManager<User> userManager, HttpClient httpClient, IConfiguration configuration, IApplicationDbContext context)
        {
            _userManager = userManager;
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        public async Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // Find the student by email
            User? student = await _userManager.FindByEmailAsync(request.Email);

            if (student == null)
            {
                return Result.Failure("The email does not belong to a registered student.");
            }

            // Retrieve Paystack secret key from configuration
            string? paystackSecretKey = _configuration["Paystack:SecretKey"];
            if (string.IsNullOrEmpty(paystackSecretKey))
            {
                return Result.Failure("Paystack secret key is not configured.");
            }

            // Set authorization header for Paystack API requests
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

            // Prepare the payment payload
            var payload = new
            {
                email = request.Email,
                amount = request.Amount * 100, // Paystack requires amount in kobo
                callback_url = _configuration["Paystack:CallbackUrl"]
            };

            // Send request to Paystack to initialize payment
            var response = await _httpClient.PostAsJsonAsync("https://api.paystack.co/transaction/initialize", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure("Unable to initiate payment with Paystack.");
            }

            // Parse the response from Paystack
            var responseData = await response.Content.ReadFromJsonAsync<PaystackInitializeResponse>(cancellationToken: cancellationToken);
            if (responseData?.Data == null)
            {
                return Result.Failure("Invalid response from Paystack.");
            }

            // Extract transaction reference and authorization URL from Paystack response
            string transactionReference = responseData.Data.Reference;
            string paymentUrl = responseData.Data.AuthorizationUrl;

            // Store transaction details in the database
            var transaction = new Transaction
            {
                StudentId = student.Id,
                TransactionReference = transactionReference,
                Email = request.Email,
                PreferredProgramme = request.PreferredProgramme,
                EducationalLevel = request.EducationalLevel,
                EmploymentStatus = request.EmploymentStatus, 
                PaymentType = request.PaymentType,
                Amount = request.Amount,
                TransactionStatus = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            // Return the payment URL to redirect the user
            return Result.Success<CreatePaymentCommand>("Authorization URL", responseData.Data);
        }
    }

    // Paystack response class
    public class PaystackInitializeResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public PaystackData Data { get; set; }

        public class PaystackData
        {
            [JsonPropertyName("authorization_url")]
            public string AuthorizationUrl { get; set; }

            [JsonPropertyName("reference")]
            public string Reference { get; set; }
        }
    }
}