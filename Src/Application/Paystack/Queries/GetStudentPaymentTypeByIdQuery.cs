using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Paystack.Queries
{
    public class GetStudentPaymentTypeByIdQuery : IRequest<Result>
    {
        public string StudentId { get; set; }
    }

    public class GetStudentPaymentTypeByIdQueryHandler : IRequestHandler<GetStudentPaymentTypeByIdQuery, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;

        public GetStudentPaymentTypeByIdQueryHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> Handle(GetStudentPaymentTypeByIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve student from UserManager
            var student = await _userManager.Users
                .Include(s => s.Programme) // Including related programme if needed
                .FirstOrDefaultAsync(u => u.Id == request.StudentId && u.UserType == UserType.Student, cancellationToken);

            if (student == null)
            {
                return Result.Failure("Student not found.");
            }

            // Retrieve the payment status and other relevant information
            var paymentType = new
            {
                ProgrammePaymentType = student.PaymentType,
                student.PaymentTypeDes,
                ProgrammeName = student.Programme
            };

            return Result.Success(paymentType);
        }
    }
}