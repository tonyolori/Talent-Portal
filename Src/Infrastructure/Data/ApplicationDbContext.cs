using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<BaseUser>(options), IApplicationDbContext
{
    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<LearningAdmin> LearningAdmins { get; set; }

    public DbSet<Module> Modules { get; set; }

    public DbSet<ModuleTask> Tasks { get; set; }
    public DbSet<Programme> Programmes { get; set; }
    
    public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

}
