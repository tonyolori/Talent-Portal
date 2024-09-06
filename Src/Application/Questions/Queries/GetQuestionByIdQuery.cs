using Application.Common.Models;  
using Application.Interfaces;  
using Domain.Entities;  
using MediatR;  
using Microsoft.EntityFrameworkCore;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading;  
using System.Threading.Tasks;  

namespace Application.Questions.Queries  
{  
    public class GetQuestionByIdQuery : IRequest<Result>  
    {  
        public int QuestionId { get; set; }  
    }  

    public class QuestionDetails  
    {  
        public int Id { get; set; }  
        public string QuestionText { get; set; }  
        public int QuizId { get; set; }  
        public List<AnswerDetails> Answers { get; set; } = new();  
    }  

    public class AnswerDetails  
    {  
        public int Id { get; set; }  
        public string AnswerText { get; set; }  
        public bool IsCorrect { get; set; }  
    }  

    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Result>  
    {  
        private readonly IApplicationDbContext _context;  

        public GetQuestionByIdQueryHandler(IApplicationDbContext context)  
        {  
            _context = context;  
        }  

        public async Task<Result> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)  
        {  
            Question? question = await _context.Questions  
                .Include(q => q.Options)  
                .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);  

            if (question == null)  
            {  
                return Result.Failure("Question not found.");  
            }  
            
            // Map to DTO  
            QuestionDetails questionDetails = new ()  
            {  
                Id = question.Id,  
                QuestionText = question.QuestionText,  
                QuizId = question.QuizId,  
                Answers = question.Options.Select(a => new AnswerDetails  
                {  
                    Id = a.Id,  
                    AnswerText = a.AnswerText,  
                    IsCorrect = a.IsCorrect  
                }).ToList()  
            };  

            return Result.Success<GetQuestionByIdQuery>( "Question details retrieved successfully.", questionDetails);  
        }  
    }  
}