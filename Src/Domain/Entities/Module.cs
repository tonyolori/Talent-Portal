using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using Domain.Common.Entities;
using Domain.Enum;


namespace Domain.Entities
{
    public class Module : BaseEntity
    {
        public string Title { get; set; }

        public string ModuleImageUrl { get; set; }

        public required string Description { get; set; }

        public required string Objectives { get; set; }

        public required string InstructorName { get; set; }

        public string InstructorId { get; set; }

        [ForeignKey(nameof(InstructorId))]

        public ModuleStatus ModuleStatus { get; set; } = ModuleStatus.Pending;

        public string ModuleStatusDes { get; set; } = ModuleStatus.Pending.ToString();


        public required int Timeframe { get; set; }

        public int ProgrammeId { get; set; }

        [ForeignKey(nameof(ProgrammeId))]
        public Programme Programme { get; set; }

        public virtual ICollection<ModuleTask> ModuleTasks { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

        public string? AdditionalResources { get; set; }
    }
}