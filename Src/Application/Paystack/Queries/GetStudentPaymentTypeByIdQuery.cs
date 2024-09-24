using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Paystack.Queries
{
    public class GetStudentPaymentTypeByIdQuery : IRequest<Result>
    {
        public string StudentId { get; set; }
    }

    public class GetStudentPaymentTypeByIdQueryHandler : IRequestHandler<GetStudentPaymentTypeByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetStudentPaymentTypeByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetStudentPaymentTypeByIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve student by ID
            var student = await _context.Students
                .Include(s => s.Programme)  // Including related programme if needed
                .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

            if (student == null)
            {
                return Result.Failure("Student not found.");
            }

            // Retrieve the payment status and other relevant information
            var paymentType = new
            {
                ProgrammePaymentType = student.ApplicationType,
                student.PaymentTypeDes,
                ProgrammeName = student.Programme
            };

            return Result.Success(paymentType);
        }
    }
}