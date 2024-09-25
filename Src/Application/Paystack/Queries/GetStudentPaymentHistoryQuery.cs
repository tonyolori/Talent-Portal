using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.Paystack.Queries
{
    public class GetStudentPaymentHistoryQuery : IRequest<Result>
    {
        public string StudentId { get; set; }
    }

    public class ApplicationHistoryDto
    {
        public DateTime ApplicationDate { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string PreferredProgramme { get; set; }
    }

    public class GetStudentPaymentHistoryQueryHandler : IRequestHandler<GetStudentPaymentHistoryQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetStudentPaymentHistoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetStudentPaymentHistoryQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the student's transaction history from the database
            List<Transaction>? transactions = await _context.Transactions
                .Where(t => t.StudentId == request.StudentId)
                .ToListAsync(cancellationToken);

            if (!transactions.Any())
            {
                return Result.Failure("No application history found.");
            }

            // Map each transaction to an ApplicationHistoryDto
            var applicationHistory = transactions.Select(MapToApplicationHistoryDto).ToList();

            return Result.Success("Application history retrieved successfully.", applicationHistory);
        }

        // Method to map a Transaction to ApplicationHistoryDto
        private ApplicationHistoryDto MapToApplicationHistoryDto(Transaction transaction)
        {
            return new ApplicationHistoryDto
            {
                ApplicationDate = transaction.CreatedAt,
                Amount = transaction.Amount,
                TransactionStatus = transaction.TransactionStatus, 
                ApplicationType = transaction.ApplicationType,
                PreferredProgramme = transaction.PreferredProgramme
            };
        }
    }
}
