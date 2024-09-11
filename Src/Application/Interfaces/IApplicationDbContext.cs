using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Student> Students { get; set; }
        
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleTask> Tasks { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<LearningAdmin> LearningAdmins { get; set; }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<SubmissionDetails> SubmissionDetails { get; set; }
        
        public DbSet<Quiz> Quizzes { get; set; }
    
        public DbSet<Question> Questions { get; set; }
    
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}