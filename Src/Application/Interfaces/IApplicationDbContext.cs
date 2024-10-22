using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;


namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> User { get; set; }
        
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleTask> Tasks { get; set; }
        
        public DbSet<Topic> Topics { get; set; }
        public DbSet<SubmissionDetails> SubmissionDetails { get; set; }
        
        public DbSet<Quiz> Quizzes { get; set; }
    
        public DbSet<Question> Questions { get; set; }
    
        public DbSet<Option> Answers { get; set; }
        
        public DbSet<StudentModule> StudentModules { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CalendarSlot> CalendarSlots { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}