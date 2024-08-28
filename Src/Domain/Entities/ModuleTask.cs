
namespace Domain.Entities
{
    public class ModuleTask
    {
        public Guid TaskId { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Set default to current UTC time
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // "Assigned", "Submitted", "Graded"
        public int? Grade { get; set; } // nullable for ungraded tasks

        public int CourseId { get; set; } // Foreign key to Course
        public Course Course { get; set; } // Navigation property for one-to-many relationship
    }
}
