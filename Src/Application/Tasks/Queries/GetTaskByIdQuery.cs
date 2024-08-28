using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Application.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<Result>
{
    public int Id { get; set; }

}

public class GetUserByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTaskByIdQuery, Result>
{
    private readonly IApplicationDbContext _context = context;


    public async Task<Result> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        ModuleTask? task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.Id);

        if (task == null)
        {
            return Result.Failure<GetTaskByIdQuery>("User Id does not exist");
        }

        return Result.Success(task);
    }
}
