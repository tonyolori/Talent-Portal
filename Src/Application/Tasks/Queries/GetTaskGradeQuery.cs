//using Application.Common.Models;
//using Application.Interfaces;
//using Domain.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Application.Tasks.Queries;

//public class GetTaskGradeQuery : IRequest<Result>
//{
//    public Guid StudentId { get; set; }
//    public Guid ModuleTaskId { get; set; }
//}

//public class GetTaskGradeQueryHandler(UserManager<Student> userManager) : IRequestHandler<GetTaskGradeQuery, Result>
//{
//    private readonly UserManager<Student> _userManager = userManager;

//    public async Task<Result> Handle(GetTaskGradeQuery request, CancellationToken cancellationToken)
//    {
//        Student? student = await _userManager.FindByIdAsync(request.StudentId.ToString());

//        if (student == null)
//        {
//            // Handle student not found scenario
//            return Result.Failure("student Not found");
//        }
//        //var grade = await _context.Grades.FirstOrDefaultAsync(g => g.TaskId == taskId, cancellationToken);
//        List<Grade>? grade = student.Grades?.Where(g => g.TaskId == request.ModuleTaskId ).ToList();

//        if (grade == null || !grade.Any())
//        {
//            // Handle task not assigned to student scenario
//            return Result.Success(new List<Grade>());
//        }

//        return Result.Success(grade);
//    }
//}