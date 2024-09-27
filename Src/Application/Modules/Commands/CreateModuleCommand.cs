using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.Modules.Commands
{
    public class CreateModuleCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public IFormFile ModuleImage { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public int ProgrammeId { get; set; }
        public string FacilitatorName { get; set; }
        public string FacilitatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Timeframe { get; set; }
        public string Progress { get; set; }
        public string AdditionalResources { get; set; }
        
        // For StudentModule
        public string StudentId { get; set; }
    }

    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public CreateModuleCommandHandler(IApplicationDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        public async Task<Result> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            // Upload the image to Cloudinary
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(request.ModuleImage.FileName, request.ModuleImage.OpenReadStream()),
                Transformation = new Transformation().Crop("limit").Width(800).Height(600).Quality("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Result.Failure("Image upload failed!");
            }

            string moduleImageUrl = uploadResult.SecureUrl.ToString();

            // Create the Module
            Module module = new ()
            {
                Title = request.Title,
                ModuleImageUrl = moduleImageUrl,
                Description = request.Description,
                Objectives = request.Objectives,
                ProgrammeId = request.ProgrammeId,
                FacilitatorName = request.FacilitatorName,
                FacilitatorId = request.FacilitatorId,
                ModuleStatus = ModuleStatus.Pending,
                ModuleStatusDes = ModuleStatus.Pending.ToString(),
                Timeframe = request.Timeframe,
                Progress = request.Progress,
                AdditionalResources = request.AdditionalResources
            };

            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
                StudentModule studentModule = new ()
                {
                    StudentId = request.StudentId,
                    ModuleId = module.Id, // After saving, module.Id will be setStudentModuleProgress = StudentModuleProgress.Pending,
                    StudentModuleProgressDes = StudentModuleProgress.Pending.ToString()
                };

                await _context.StudentModules.AddAsync(studentModule, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


                var studentModuleDetails = new
                {
                    module,
                    studentModule

                };

            return Result.Success<CreateModuleCommand>("Module and StudentModule created successfully!", studentModuleDetails);
        }
    }
}
