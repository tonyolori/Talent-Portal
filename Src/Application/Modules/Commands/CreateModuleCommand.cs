using System.Text.Json.Serialization;
using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.Commands
{
    public class CreateModuleCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public IFormFile ModuleImage { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public int ProgrammeId { get; set; }
        
        [JsonIgnore] 
        public string? InstructorId { get; set; }
        
        public int Timeframe { get; set; }
        
        public string? AdditionalResources { get; set; }
        
 
    }

    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly Cloudinary _cloudinary;

        public CreateModuleCommandHandler(IApplicationDbContext context, UserManager<User> userManager,Cloudinary cloudinary)
        {
            _context = context;
            _userManager = userManager;
            _cloudinary = cloudinary;
        }

        public async Task<Result> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            
            // Find the instructor using UserManager by their Id
            User? instructor = await _userManager.FindByIdAsync(request.InstructorId);

            if (instructor == null || instructor.UserType != UserType.Instructor)
            {
                return Result.Failure("Instructor not found.");
            }
            // Upload the image to Cloudinary
            ImageUploadParams uploadParams = new()
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
                InstructorName = instructor.FirstName,
                InstructorId = instructor.Id,
                Timeframe = request.Timeframe,
                AdditionalResources = request.AdditionalResources
            };

            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success<CreateModuleCommand>("Module created successfully!", module);
        }
    }
}
