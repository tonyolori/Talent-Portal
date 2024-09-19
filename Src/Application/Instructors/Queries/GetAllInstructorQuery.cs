using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Queries;

using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;

public class GetAllInstructorsQuery : IRequest<Result>
{
  public class GetAllInstructorsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAllInstructorsQuery, Result>
  {
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
    {
      List<Instructor>? instructors = await _context.Instructors.ToListAsync(cancellationToken);

      if (!instructors.Any())
      {
        return Result.Failure<Instructor>("Instructor not found.");
      }
      
      var response = new  
      {  
        instructors , 
        InstructorsLength = instructors.Count,  
      };  
      
      return Result.Success<GetAllInstructorsQuery>("All Instructors", response);
    }
  }
}
