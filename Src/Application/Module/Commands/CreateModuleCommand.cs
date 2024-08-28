using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Application.Modules.Commands
{
    public class CreateModuleCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public IFormFile ModuleImage { get; set; }
        public List<string> Topics { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public string FacilitatorName { get; set; }
        public string FacilitatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Timeframe { get; set; }
        public string Progress { get; set; }
        public string AdditionalResources { get; set; }
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
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(request.ModuleImage.FileName, request.ModuleImage.OpenReadStream()),
                Transformation = new Transformation().Crop("limit").Width(800).Height(600).Quality("auto")
            };

            UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Result.Failure("Image upload failed!");
            }

            string moduleImageUrl = uploadResult.SecureUrl.ToString();

            // Create the module
            Module module = new()
            {
                Title = request.Title,
                ModuleImageUrl = moduleImageUrl,
                Topics = request.Topics,
                Description = request.Description,
                Objectives = request.Objectives,
                FacilitatorName = request.FacilitatorName,
                FacilitatorId = request.FacilitatorId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Timeframe = request.Timeframe,
                Progress = request.Progress,
                AdditionalResources = request.AdditionalResources
            };

            // Save the module to the database
            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Module created successfully!", module);
        }
    }
}
