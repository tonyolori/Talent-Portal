using Application.Common.Models;  
using MediatR;  
using Application.Interfaces;  
using Domain.Entities;  
using Microsoft.EntityFrameworkCore;  
using System.Linq;  
using System.Threading;  
using System.Threading.Tasks;  
using Application.Dto;  

namespace Application.Questions.Queries  
{  
    public class GetQuestionByIdQuery : IRequest<Result>  
    {  
        public int QuestionId { get; set; }  
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
                .SingleOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);  

            if (question == null)  
            {  
                return Result.Failure<QuestionDto>("Question not found.");  
            }  

            
            string? correctOptionText = question.Options  
                .Where(o => o.Id == question.CorrectOptionId)  
                .Select(o => o.AnswerText)   
                .FirstOrDefault();  

       
            QuestionDto questionDto = new ()  
            {  
                Id = question.Id,  
                QuestionText = question.QuestionText,  
                CorrectOptionText = correctOptionText,  
                Options = question.Options.Select(o => new OptionDto  
                {  
                    Id = o.Id,  
                    OptionText = o.AnswerText   
                }).ToList()  
            };  

            return Result.Success<GetQuestionByIdQuery>("Question details", questionDto);  
        }  
    }  
}