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
        
        public int Timeframe { get; set; }
        
        public string? AdditionalResources { get; set; }
        
 
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
                AdditionalResources = request.AdditionalResources
            };

            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success<CreateModuleCommand>("Module and created successfully!", module);
        }
    }
}
