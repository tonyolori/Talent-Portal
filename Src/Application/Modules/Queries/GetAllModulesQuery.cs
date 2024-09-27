using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Queries
{
    public class GetAllModulesQuery : IRequest<Result>
    {
    }

    public class GetAllModulesQueryHandler : IRequestHandler<GetAllModulesQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllModulesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
        {
            List<ModuleDetailsDto>? modules = await _context.Modules
                .Include(m => m.Topics)
                .Include(m => m.ModuleTasks)
                .Include(m => m.Quizzes)
                .Select(m => new ModuleDetailsDto
                {
                    ModuleId = m.Id,
                    Title = m.Title,
                    ModuleImageUrl = m.ModuleImageUrl,
                    Description = m.Description,
                    Objectives = m.Objectives,
                    FacilitatorName = m.FacilitatorName,
                    FacilitatorId = m.FacilitatorId,
                    Timeframe = m.Timeframe,
                    ProgrammeId = m.ProgrammeId,
                    Progress = m.Progress,
                    AdditionalResources = m.AdditionalResources,
                    TotalTopics = m.Topics.Count, 
                    TotalModuleTasks = m.ModuleTasks.Count, 
                    TotalQuizzes = m.Quizzes.Count 
                })
                .ToListAsync(cancellationToken);

            if (!modules.Any())
            {
                return Result.Success("No modules found.");
            }

            var response = new
            {
                Modules = modules,
                TotalModules = modules.Count
            };

            return Result.Success(response);
        }
    }

    // DTO for Module Details
    public class ModuleDetailsDto
    {
        public int ModuleId { get; set; }
        public string Title { get; set; }
        public string ModuleImageUrl { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public string FacilitatorName { get; set; }
        public string FacilitatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Timeframe { get; set; }
        public int ProgrammeId { get; set; }
        public string Progress { get; set; }
        public string? AdditionalResources { get; set; }
        public int TotalTopics { get; set; } 
        public int TotalModuleTasks { get; set; }
        public int TotalQuizzes { get; set; }
    }
}
