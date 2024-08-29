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
        
        public DbSet<Programme> Programmes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}