using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;


namespace Domain.Entities
{
    public class Module : BaseEntity
    {
        public string Title { get; set; }
        
        public string ModuleImageUrl { get; set; }

        public ICollection<Topic> Topics { get; set; } = new List<Topic>();

        public required string Description { get; set; }

        public required string Objectives { get; set; }

        public required string FacilitatorName { get; set; }

        public required string FacilitatorId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public required string Timeframe { get; set; }

        public int ProgrammeId { get; set; }

        [ForeignKey(nameof(ProgrammeId))]
        public Programme Programme { get; set; }

        public virtual ICollection<ModuleTask> ModuleTasks { get; set; }

        public string Progress { get; set; }

        public string? AdditionalResources { get; set; }
    }
}