using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Common.Enum;
using Domain.Entities;

namespace Application.Topics.Commands
{
    public class UpdateTopicCommand : IRequest<Result>
    {
        public int Id { get; set; }         
        public string Title { get; set; }
        public string MainContent { get; set; }
        public string? SubContent { get; set; }
        public TopicStatus Status { get; set; }
        public int ModuleId { get; set; }
    }

    public class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTopicCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            // Find the topic by ID
            Topic? topic = await _context.Topics.FindAsync(request.Id);

            if (topic == null)
            {
                return Result.Failure("Topic not found.");
            }

            // Update the topic properties
            topic.Title = request.Title;
            topic.MainContent = request.MainContent;
            topic.SubContent = request.SubContent;
            topic.Status = request.Status;
            topic.ModuleId = request.ModuleId;

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<UpdateTopicCommand>("Topic updated successfully!", topic);
        }
    }
}