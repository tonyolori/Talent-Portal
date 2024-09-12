using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Application.Tasks.Queries;

public class GetAllTasksQuery : IRequest<Result>
{

}

public class GetAllTasksQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAllTasksQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        List<ModuleTask>? tasks = await _context.Tasks.ToListAsync();
  
        // Return the user or null if not found
        return Result.Success<GetAllTasksQuery>("Tasks retrieved successfully.", tasks);
    }
}
