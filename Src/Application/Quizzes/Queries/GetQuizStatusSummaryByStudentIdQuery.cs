using Application.Common.Models;  
using MediatR;  
using Application.Interfaces;  
using Microsoft.EntityFrameworkCore;  
using System.Linq;  
using Domain.Entities;  
using Domain.Enum;  

namespace Application.Quizzes.Queries  
{  
    public class GetQuizStatusSummaryByStudentIdQuery : IRequest<Result>  
    {  
        // Update the StudentId to be of type Guid  
        public string StudentId { get; set; }  
    }  

    public class GetQuizStatusSummaryByStudentIdQueryHandler : IRequestHandler<GetQuizStatusSummaryByStudentIdQuery, Result>  
    {  
        private readonly IApplicationDbContext _context;  

        public GetQuizStatusSummaryByStudentIdQueryHandler(IApplicationDbContext context)  
        {  
            _context = context;  
        }  

        public async Task<Result> Handle(GetQuizStatusSummaryByStudentIdQuery request, CancellationToken cancellationToken)  
        {  
            // Use the Guid directly in the query  
            List<Quiz>? quizzes = await _context.Quizzes  
                .Where(q => q.StudentId == request.StudentId)  
                .ToListAsync(cancellationToken);  

            if (!quizzes.Any())  
            {  
                return Result.Failure($"No quizzes found for student with ID {request.StudentId}.");  
            }  

            int totalQuizzes = quizzes.Count;  
            int totalPassed = quizzes.Count(q => q.QuizStatus == QuizStatus.Passed);  
            int totalFailed = quizzes.Count(q => q.QuizStatus == QuizStatus.Failed);  
            int totalNotStarted = quizzes.Count(q => q.QuizStatus == QuizStatus.NotStarted);  

            var summary = new  
            {  
                TotalQuizzes = totalQuizzes,  
                TotalPassed = totalPassed,  
                TotalFailed = totalFailed,  
                TotalNotStarted = totalNotStarted  
            };  

            return Result.Success<GetQuizStatusSummaryByStudentIdQuery>("Quiz status summary retrieved successfully.", summary);  
        }  
    }  
}